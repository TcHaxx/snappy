namespace TcHaxx.Snappy.Verifier.Options;

public interface IVerifierOptions
{
    /// <summary>
    /// Directory to store all *.verified.txt.
    /// </summary>
    string VerifyDirectory { get; init; }

    /// <summary>
    /// Compact diff output, e.g. for TcUnit where max. ADS log string = 255.
    /// </summary>
    bool CompactDiff { get; init; }

    /// <summary>
    /// Floating point precision for REAL/LREAL.
    /// </summary>
    ushort FloatingPointPrecision { get; init; }
}
