using CommandLine;
using TcHaxx.Snappy.TcADS.Options;
using TcHaxx.Snappy.Verifier.Options;

namespace TcHaxx.Snappy.CLI;

[Verb("verify", false, HelpText = "Verify.")]
public class VerifyOptions : BaseOptions, ITcAdsOptions, IVerifierOptions
{
    [Option('d', "dir", Default = Verifier.Constants.DEFAULT_OPTION_VERIFIED_DIRECTORY, Required = false, HelpText = "Directory of verified snapshot files.")]
    public string VerifyDirectory { get; init; } = string.Empty;
    [Option('c', "compact-diff", Default = Verifier.Constants.DEFAULT_OPTION_COMPACT_DIFF, Required = false, HelpText = "Diff output as compact as possible.")]
    public bool CompactDiff { get; init; }

    [Option('p', "port", Default = TcADS.Constants.DEFAULT_AMS_PORT, Required = false, HelpText = "AmsPort of the Server.")]
    public ushort AdsPort { get; init; }
}
