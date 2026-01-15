using Microsoft.Win32;

namespace TcHaxx.Snappy.CLI.Installer;

internal static class RegistryHelper
{
    internal const string DEFAULT_VERSION = "3.1.4026.0";
    internal const string REGISTRY_TWONCAT31_KEY = "SOFTWARE\\WOW6432Node\\Beckhoff\\TwinCAT3\\3.1";
    internal static string GetTwincatInstallDirectory()
    {
        var regKey = Registry.LocalMachine.OpenSubKey(REGISTRY_TWONCAT31_KEY);
        return (regKey?.GetValue("InstallDir") as string) ?? string.Empty;
    }
}
