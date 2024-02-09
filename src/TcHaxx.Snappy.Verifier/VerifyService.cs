using System.Reflection;
using Serilog;
using TcHaxx.Snappy.Common.RPC;
using TcHaxx.Snappy.Common.RPC.Attributes;
using TcHaxx.Snappy.Common.Verify;
using TcHaxx.Snappy.Verifier.Options;

namespace TcHaxx.Snappy.Verifier;

public class VerifyService : IVerifyService
{
    private readonly ILogger _Logger;

    public VerifyService(IRpcMethodDescriptor rpcMethodDescriptor, ILogger logger)
    {
        rpcMethodDescriptor.Register(this);
        _Logger = logger;
        Options = new DefaultOptions();
    }

    public IVerifierOptions Options { get; set; }

    public VerificationResult Verify(
        [String(Common.Constants.DEFAULT_STRING_PARAMETER_LENGTH)] string testSuiteName,
        [String(Common.Constants.DEFAULT_STRING_PARAMETER_LENGTH)] string testName,
        [String(Common.Constants.DEFAULT_JSON_PARAMETER_LENGTH)] string jsonToVerify)
    {

        // https://github.com/orgs/VerifyTests/discussions/598
        // https://github.com/orgs/VerifyTests/discussions/611
        try
        {
            // TODO: See if global VerifySettings should be used.
            //       Or, if a client (TcHaxx.Snappy) sets/configures these settings on the fly.
            var settings = new VerifySettings();

            var directory = Path.IsPathRooted(Options.VerifyDirectory) ? Options.VerifyDirectory
                : Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, Options.VerifyDirectory);

            settings.UseDirectory(directory);
            settings.DisableRequireUniquePrefix();
            settings.UseDiffPlex(VerifyTests.DiffPlex.OutputType.Compact);
            var iv = new InnerVerifier(Assembly.GetExecutingAssembly().Location, settings, testSuiteName, testName, null, new PathInfo());
            var result = iv.VerifyJson(jsonToVerify).Result;

            return new VerificationResult { Diff = string.Empty, HResult = 0 };
        }
        catch (Exception ex)
        {
            _Logger?.Fatal(ex, "Exception: {ExceptionMessage}", ex.Message);

            var diff = ex.InnerException?.Message ?? ex.Message;
            return Options.CompactDiff ? new VerificationResult { Diff = diff, HResult = ex.HResult }.ToCompactDiff() : new VerificationResult { Diff = diff, HResult = ex.HResult };
        }

    }
}
