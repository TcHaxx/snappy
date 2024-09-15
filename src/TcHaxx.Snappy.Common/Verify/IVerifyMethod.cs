using TcHaxx.Snappy.Common.RPC;
using TcHaxx.Snappy.Common.RPC.Attributes;

namespace TcHaxx.Snappy.Common.Verify;

public interface IVerifyMethod : IRpcMethodMarker
{
    public VerificationResult Verify(
        [String(Constants.DEFAULT_TEST_NAMES_LENGTH)] string testSuiteName,
        [String(Constants.DEFAULT_TEST_NAMES_LENGTH)] string testName,
        [String(16384)] string jsonToVerify);
}
