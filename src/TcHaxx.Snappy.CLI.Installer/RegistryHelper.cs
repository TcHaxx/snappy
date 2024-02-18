using Microsoft.Win32;

namespace TcHaxx.Snappy.CLI.Installer;
internal static class RegistryHelper
{
    internal static string GetTwincatInstallDirectory()
    {
        var regKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Beckhoff\\TwinCAT3\\3.1");
        return (regKey?.GetValue("InstallDir") as string) ?? string.Empty;
    }
}
