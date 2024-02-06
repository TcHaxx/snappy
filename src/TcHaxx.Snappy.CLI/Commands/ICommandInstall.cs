namespace TcHaxx.Snappy.CLI.Commands;

internal interface ICommandInstall
{
    Task<int> RunAndReturnExitCode(InstallOptions options);
}
