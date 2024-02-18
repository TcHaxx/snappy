using TcHaxx.Snappy.CLI.CLI;
using TcHaxx.Snappy.Common;

namespace TcHaxx.Snappy.CLI.Commands;

internal interface ICommandVerify
{
    Task<ExitCodes> RunAndReturnExitCode(VerifyOptions options);
}
