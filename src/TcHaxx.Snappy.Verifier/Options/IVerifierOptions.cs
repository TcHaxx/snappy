namespace TcHaxx.Snappy.Verifier.Options;

public interface IVerifierOptions
{
    /// <summary>
    /// Directory to store all *.verified.txt.
    /// </summary>
    public string VerifyDirectory { get; init; }

    /// <summary>
    /// Compact diff output, e.g. for TcUnit where max. ADS log string = 255.
    /// </summary>
    public bool CompactDiff { get; init; }
}
