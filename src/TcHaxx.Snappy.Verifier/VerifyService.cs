using System.Reflection;
using Serilog;
using TcHaxx.Snappy.Common;
using TcHaxx.Snappy.Common.RPC;
using TcHaxx.Snappy.Common.RPC.Attributes;
using TcHaxx.Snappy.Common.Verify;

namespace TcHaxx.Snappy.Verifier;

public class VerifyService : IVerifyMethod
{
    private readonly ILogger _Logger;

    public VerifyService(IRpcMethodDescriptor rpcMethodDescriptor, ILogger logger)
    {
        rpcMethodDescriptor.Register(this);
        _Logger = logger;
    }

    public VerificationResult Verify(
        [String(Constants.DEFAULT_STRING_PARAMETER_LENGTH)] string testSuiteName,
        [String(Constants.DEFAULT_STRING_PARAMETER_LENGTH)] string testName,
        [String(Constants.DEFAULT_JSON_PARAMETER_LENGTH)] string jsonToVerify)
    {

        // https://github.com/orgs/VerifyTests/discussions/598
        // https://github.com/orgs/VerifyTests/discussions/611
        try
        {
            // TODO: See if global VerifySettings should be used.
            //       Or, if a client (TcHaxx.Snappy) sets/configures these settings on the fly.
            var settings = new VerifySettings();
            settings.UseDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!);
            settings.DisableRequireUniquePrefix();
            settings.UseDiffPlex(VerifyTests.DiffPlex.OutputType.Compact);
            var iv = new InnerVerifier(Assembly.GetExecutingAssembly().Location, settings, testSuiteName, testName, null, new PathInfo());
            var result = iv.VerifyJson(jsonToVerify).Result;

            return new VerificationResult { Diff = string.Empty, HResult = 0 };
        }
        catch (Exception ex)
        {
            _Logger?.Fatal(ex, "Exception: {ExceptionMessage}", ex.Message);

            // TODO: Reduce verbose diff output, further.
            return new VerificationResult { Diff = ex.InnerException?.Message ?? ex.Message, HResult = ex.HResult };
        }

    }
}
