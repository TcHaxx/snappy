using Serilog;
using TcHaxx.Snappy.CLI.Installer.Options;
using TcHaxx.Snappy.Common;

namespace TcHaxx.Snappy.CLI.Installer;
public interface IInstallerService
{
    public Task<ExitCodes> Install(IInstallerOptions options, ILogger? logger);
}
