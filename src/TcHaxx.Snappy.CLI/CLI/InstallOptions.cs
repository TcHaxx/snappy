using CommandLine;
using TcHaxx.Snappy.CLI.Installer.Options;

namespace TcHaxx.Snappy.CLI.CLI;

[Verb("install", false, HelpText = "Install snappy.library and dependencies.")]
public class InstallOptions : BaseOptions, IInstallerOptions
{
    ///<inheritdoc />
    [Option("tc-profile", Default = Installer.Constants.DEFAULT_OPTION_TCPROFILE, Required = false, HelpText = "TwinCAT profile to use, i.e \"latest\" or specific version \"TwinCAT PLC Control_Build_4024.54\".")]
    public string TcProfile { get; init; } = string.Empty;

    ///<inheritdoc />
    [Option("tool-path", Default = Installer.Constants.DEFAULT_OPTION_TOOLSPATH, Required = false, HelpText = "Directory, where dotnet global tools are installed.")]
    public string ToolsPath { get; init; } = string.Empty;
}
