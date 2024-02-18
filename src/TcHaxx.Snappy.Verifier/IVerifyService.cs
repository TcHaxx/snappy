using TcHaxx.Snappy.Common.Verify;
using TcHaxx.Snappy.Verifier.Options;

namespace TcHaxx.Snappy.Verifier;

public interface IVerifyService : IVerifyMethod
{
    public IVerifierOptions Options { get; set; }
}
