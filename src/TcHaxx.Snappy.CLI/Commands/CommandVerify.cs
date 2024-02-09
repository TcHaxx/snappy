using Serilog;
using TcHaxx.Snappy.Common.Verify;
using TcHaxx.Snappy.TcADS;
using TcHaxx.Snappy.Verifier;
using TcHaxx.Snappy.Verifier.Options;

namespace TcHaxx.Snappy.CLI.Commands;

internal class CommandVerify : ICommandVerify
{
    private readonly IEnumerable<IVerifyService> _VerifyServices;
    private readonly ISymbolicServerFactory _SymbolicServerFactory;
    private readonly ILogger? _Logger;

    public CommandVerify(IEnumerable<IVerifyService> verifyServices, ISymbolicServerFactory symbolicServerFactory, ILogger? logger)
    {
        _VerifyServices = verifyServices;
        _SymbolicServerFactory = symbolicServerFactory ?? throw new ArgumentNullException(nameof(symbolicServerFactory));
        _Logger = logger;
    }

    public async Task<int> RunAndReturnExitCode(VerifyOptions options)
    {
        foreach (var service in _VerifyServices)
        {
            service.Options = options;
        }

        using var symbolicServer = _SymbolicServerFactory.CreateSymbolicServer(options.AdsPort, nameof(CommandVerify));

        var adsRetVal = await symbolicServer.ConnectServerAndWaitAsync(new CancellationToken());

        return 0;
    }
}
