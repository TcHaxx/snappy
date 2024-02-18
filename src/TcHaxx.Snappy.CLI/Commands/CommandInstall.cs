using Serilog;
using TcHaxx.Snappy.CLI.CLI;

namespace TcHaxx.Snappy.CLI.Commands;

internal class CommandInstall(ILogger? logger) : ICommandInstall
{
    private readonly ILogger? _logger = logger?.ForContext<CommandInstall>();

    public async Task<int> RunAndReturnExitCode(InstallOptions options)
    {
        _logger?.Information("Installing ...");
        await Task.Delay(1000);
        return 0;
    }

}
