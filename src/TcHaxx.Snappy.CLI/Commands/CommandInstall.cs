using Serilog;
using TcHaxx.Snappy.CLI.CLI;
using TcHaxx.Snappy.CLI.Installer;
using TcHaxx.Snappy.Common;

namespace TcHaxx.Snappy.CLI.Commands;

internal class CommandInstall(ILogger? logger) : ICommandInstall
{
    private readonly ILogger? _logger = logger?.ForContext<CommandInstall>();
    private readonly IInstallerService _installerService = new InstallerService();

    public async Task<ExitCodes> RunAndReturnExitCode(InstallOptions options)
    {
        return await _installerService.Install(options, _logger);
    }
}
