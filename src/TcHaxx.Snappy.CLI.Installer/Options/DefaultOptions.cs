namespace TcHaxx.Snappy.CLI.Installer.Options;
internal class DefaultOptions : IInstallerOptions
{
    ///<inheritdoc/> 
    public string TcProfile { get; init; } = Constants.DEFAULT_OPTION_TCPROFILE;

    ///<inheritdoc/> 
    public string ToolsPath { get; init; } = Constants.DEFAULT_OPTION_TOOLSPATH;
}
