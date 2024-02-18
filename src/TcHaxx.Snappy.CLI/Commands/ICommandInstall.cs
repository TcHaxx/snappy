using TcHaxx.Snappy.CLI.CLI;
using TcHaxx.Snappy.Common;

namespace TcHaxx.Snappy.CLI.Commands;

internal interface ICommandInstall
{
    Task<ExitCodes> RunAndReturnExitCode(InstallOptions options);
}
