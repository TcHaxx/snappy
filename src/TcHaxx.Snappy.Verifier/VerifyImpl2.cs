using TcHaxx.Snappy.Common.RPC;
using TcHaxx.Snappy.Common.RPC.Attributes;
using TcHaxx.Snappy.Common.Verify;

namespace TcHaxx.Snappy.Verifier;

/// <summary>
/// Example, second Verify method
/// </summary>
/// <remarks>If more then one <see cref="IVerifyMethod.Verify(string, string, string)"/> method is used, <see cref="AliasAttribute"/> must be present!</remarks>
public class VerifyImpl2 : IVerifyMethod
{
    public VerifyImpl2(IRpcMethodDescriptor rpcMethodDescriptor)
    {
        rpcMethodDescriptor.Register(this);
    }

    [Alias("Verify2")]
    public VerificationResult Verify([String(80)] string testSuiteName, [String(80)] string testName, [String(16384)] string jsonToVerify)
    {
        return new VerificationResult { Diff = "Second", HResult = 2 };
    }
}
