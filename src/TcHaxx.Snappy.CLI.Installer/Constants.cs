namespace TcHaxx.Snappy.CLI.Installer;
public static class Constants
{
    public const string TC31_PROFILES_DIRECTORY = @"Components\Plc\Profiles";
    public const string TC31_REPTOOL_EXE = @"Components\Plc\Common\RepTool.exe";
    public const string TC31_GLOB_PROFILE = "TwinCAT PLC Control_Build_*.profile";

    public const string DEFAULT_OPTION_TCPROFILE = "latest";
    public const string DEFAULT_OPTION_TOOLSPATH = @"%USERPROFILE%\.dotnet\tools\.store\tchaxx.snappy.cli";

    public static readonly int REPTOOL_EXE_TIMEOUT_MS = 180_000;
}
