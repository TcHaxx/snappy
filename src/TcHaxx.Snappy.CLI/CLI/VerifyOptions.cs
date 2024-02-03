using CommandLine;
using TcHaxx.Snappy.TcADS;
using TcHaxx.Snappy.TcADS.Options;
using TcHaxx.Snappy.Verifier.Options;

namespace TcHaxx.Snappy.CLI;

[Verb("verify", false, HelpText = "Verify.")]
public class VerifyOptions : BaseOptions, ITcAdsOptions, IVerifierOptions
{
    [Option('d', "dir", Default = "./Verified", Required = false, HelpText = "Directory of verified snapshot files.")]
    public string VerifyDirectory { get; init; } = string.Empty;

    [Option('p', "port", Default = Constants.DEFAULT_AMS_PORT, Required = false, HelpText = "AmsPort of the Server.")]
    public ushort AdsPort { get; init; }
}
