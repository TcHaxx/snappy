namespace TcHaxx.Snappy.Verifier.Options;

internal class DefaultOptions : IVerifierOptions
{
    ///<inheritdoc/> 
    public string VerifyDirectory { get; init; } = Constants.DEFAULT_OPTION_VERIFIED_DIRECTORY;
    ///<inheritdoc/> 
    public bool CompactDiff { get; init; } = Constants.DEFAULT_OPTION_COMPACT_DIFF;
    ///<inheritdoc/> 
    public ushort FloatingPointPrecision { get; init; } = Constants.DEFAULT_OPTION_FP_PRECISION;
}
