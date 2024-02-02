using Serilog;
using System.Reflection;
using TcHaxx.Snappy.TcADS;

namespace TcHaxx.Snappy.CLI.Commands;

internal class CommandVerify : ICommandVerify
{
    private readonly ISymbolicServerFactory symbolicServerFactory;
    private readonly ILogger? _Logger;

    public CommandVerify(ISymbolicServerFactory symbolicServerFactory, ILogger? logger)
    {
        this.symbolicServerFactory = symbolicServerFactory ?? throw new ArgumentNullException(nameof(symbolicServerFactory));
        _Logger = logger;
    }

    public async Task<int> RunAndReturnExitCode(VerifyOptions options)
    {

        using var symbolicServer = symbolicServerFactory.CreateSymbolicServer(options.AdsPort, nameof(CommandVerify));

        var adsRetVal = await symbolicServer.ConnectServerAndWaitAsync(new CancellationToken());

        // https://github.com/orgs/VerifyTests/discussions/598
        // https://github.com/orgs/VerifyTests/discussions/611
        try
        {

            var settings = new VerifySettings();
            settings.UseDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!);
            var iv = new InnerVerifier(Assembly.GetExecutingAssembly().Location, settings, "the type", "the method", null, new PathInfo());
            var result = await iv.Verify("huhu");

            Console.WriteLine(result);
        }
        catch (Exception ex)
        {
            _Logger?.Fatal(ex, "Exception: {ExceptionMessage}", ex.Message);
            return (int)ExitCodes.E_EXCEPTION;
        }
        await Task.Delay(1000);
        return 0;
    }
}
