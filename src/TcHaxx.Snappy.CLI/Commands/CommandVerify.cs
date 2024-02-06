using System.Reflection;
using Serilog;
using TcHaxx.Snappy.Common.Verify;
using TcHaxx.Snappy.TcADS;

namespace TcHaxx.Snappy.CLI.Commands;

internal class CommandVerify : ICommandVerify
{
    private readonly IEnumerable<IVerifyMethod> verifyMethods;
    private readonly ISymbolicServerFactory symbolicServerFactory;
    private readonly ILogger? _Logger;

    public CommandVerify(IEnumerable<IVerifyMethod> verifyMethods, ISymbolicServerFactory symbolicServerFactory, ILogger? logger)
    {
        this.verifyMethods = verifyMethods;
        this.symbolicServerFactory = symbolicServerFactory ?? throw new ArgumentNullException(nameof(symbolicServerFactory));
        _Logger = logger;
    }

    public async Task<int> RunAndReturnExitCode(VerifyOptions options)
    {

        using var symbolicServer = symbolicServerFactory.CreateSymbolicServer(options.AdsPort, nameof(CommandVerify));

        var adsRetVal = await symbolicServer.ConnectServerAndWaitAsync(new CancellationToken());

        return 0;
    }
}
