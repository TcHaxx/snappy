using System.Reflection;
using Serilog;
using TcHaxx.Snappy.Common.RPC;
using TcHaxx.Snappy.Common.RPC.Attributes;
using TcHaxx.Snappy.Common.Verify;

namespace TcHaxx.Snappy.Verifier;

public class ExampleAnotherVerifyService : IVerifyMethod
{
    private readonly ILogger _Logger;

    public ExampleAnotherVerifyService(IRpcMethodDescriptor rpcMethodDescriptor, ILogger logger)
    {
        rpcMethodDescriptor.Register(this);
        _Logger = logger;
    }

    [Alias("AliasVerifyAttributeNeededHere!")]
    public VerificationResult Verify([String(80)] string testSuiteName, [String(80)] string testName, [String(4096)] string jsonToVerify)
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
            settings.UseDiffPlex(VerifyTests.DiffPlex.OutputType.Full);
            var iv = new InnerVerifier(Assembly.GetExecutingAssembly().Location, settings, testSuiteName, testName, null, new PathInfo());
            var result = iv.VerifyJson(jsonToVerify).Result;

            return new VerificationResult { Diff = string.Empty, HResult = 0 };
        }
        catch (Exception ex)
        {
            _Logger?.Fatal(ex, "Exception: {ExceptionMessage}", ex.Message);
            return new VerificationResult { Diff = /*ex.InnerException?.Message ??*/ ex.Message, HResult = ex.HResult };
        }

    }
}
