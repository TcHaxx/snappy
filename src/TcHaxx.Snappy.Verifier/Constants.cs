namespace TcHaxx.Snappy.Verifier;

public static class Constants
{
    /// <summary>
    /// Default directory to store all *.verified.txt.
    /// </summary>
    public const string DEFAULT_OPTION_VERIFIED_DIRECTORY = "./TcHaxx.Snappy.Verified";

    /// <summary>
    /// Diff output as compact as possible.
    /// </summary>
    public const bool DEFAULT_OPTION_COMPACT_DIFF = true;

    /// <summary>
    /// Default floating point precision for REAL/LREAL.
    /// </summary>
    public const ushort DEFAULT_OPTION_FP_PRECISION = 5;

    /// <summary>
    /// Default no auto-accept any changes.
    /// </summary>
    public const bool DEFAULT_OPTION_AUTO_VERIFY = false;
}
