using Serilog;
using TcHaxx.Snappy.CLI.CLI;
using TcHaxx.Snappy.Common;
using TcHaxx.Snappy.TcADS;
using TcHaxx.Snappy.Verifier;

namespace TcHaxx.Snappy.CLI.Commands;

internal class CommandVerify(IEnumerable<IVerifyService> verifyServices, ISymbolicServerFactory symbolicServerFactory, ILogger? logger) : ICommandVerify
{
    private readonly IEnumerable<IVerifyService> _verifyServices = verifyServices;
    private readonly ISymbolicServerFactory _symbolicServerFactory = symbolicServerFactory ?? throw new ArgumentNullException(nameof(symbolicServerFactory));
    private readonly ILogger? _logger = logger;

    public async Task<ExitCodes> RunAndReturnExitCode(VerifyOptions options)
    {
        foreach (var service in _verifyServices)
        {
            service.Options = options;
        }

        _logger?.Information("Creating AdsSymbolicServer...");
        using var symbolicServer = _symbolicServerFactory.CreateSymbolicServer(options.AdsPort, nameof(CommandVerify));
        _logger?.Information("Starting AdsSymbolicServer...");
        var adsRetVal = await symbolicServer.ConnectServerAndWaitAsync(new CancellationToken());

        return (ExitCodes)adsRetVal;
    }
}
