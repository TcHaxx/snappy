using System.Text;
using TcHaxx.Snappy.Common.RPC.Attributes;

namespace TcHaxx.Snappy.Common.Verify;

public interface IVerifyMethod
{

    VerificationResult Verify([String(80)] string testSuiteName, [String(80)] string testName, [String(16384)] string jsonToVerify);
}
