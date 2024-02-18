namespace TcHaxx.Snappy.CLI.Installer.Options;

public interface IInstallerOptions
{
    /// <summary>
    /// TwinCAT profile to use, i.e "latest" or specific version "TwinCAT PLC Control_Build_4024.54"
    /// </summary>
    public string TcProfile { get; init; }

    /// <summary>
    /// Directory, where Dotnet global tools are installed, e.g. %USERPROFILE%\.dotnet\tools.
    /// </summary>
    public string ToolsPath { get; init; }

}
