using Serilog;
using TcHaxx.Snappy.CLI.Installer.Options;
using TcHaxx.Snappy.Common;

namespace TcHaxx.Snappy.CLI.Installer;
public class InstallerService : IInstallerService
{
    public async Task<ExitCodes> Install(IInstallerOptions options, ILogger? logger)
    {

        var tcProfile = TwincatProfileFactory.GetTwinCatProfile(options, logger);
        if (tcProfile is null)
        {
            return ExitCodes.E_ERROR;
        }


        var expandedPath = Environment.ExpandEnvironmentVariables(options.ToolsPath);
        var sourceDirectoryInfo = new DirectoryInfo(expandedPath ?? options.ToolsPath);

        var exitCode = await RepToolProcess.RunRepToolAsync(tcProfile, sourceDirectoryInfo, logger);
        return exitCode;
    }
}
