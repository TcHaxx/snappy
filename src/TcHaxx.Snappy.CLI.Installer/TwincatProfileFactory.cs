using System.Diagnostics.CodeAnalysis;
using Serilog;
using TcHaxx.Snappy.CLI.Installer.Options;

namespace TcHaxx.Snappy.CLI.Installer;
internal static class TwincatProfileFactory
{
    internal static TwincatProfile? GetTwinCatProfile(IInstallerOptions options, ILogger? logger)
    {
        if (!IsDefaultProfile(options))
        {
            logger?.Information("Using provided TwinCAT profile \"{TwinCatProfile}\"", options.TcProfile);
            return new TwincatProfile(options.TcProfile);
        }

        if (!TryGetTwincatInstallDirectory(out var tcInstallDir))
        {
            logger?.Error("Couldn't read TwinCAT installation directory from Registry.");
            return default;
        }

        if (!TryGetProfilesDirectory(tcInstallDir, out var profilesDir))
        {
            logger?.Error("Directory \"{TwinCatProfileDirectory}\" doesn't exist.", profilesDir);
            return default;
        }

        if (!TryGetProfileToUse(profilesDir, out var profileToUse))
        {
            logger?.Error("Couldn't find any profiles \"{TwinCatProfileGlob}\" in \"{TwinCatProfileDirectory}\".",
                Constants.TC31_GLOB_PROFILE, profilesDir);
            return default;
        }

        logger?.Information("Using TwinCAT profile \"{TwinCatProfile}\"", profileToUse);
        return new TwincatProfile(Path.GetFileNameWithoutExtension(profileToUse));
    }

    internal static bool IsDefaultProfile(IInstallerOptions options)
    {
        return options.TcProfile.Equals(Constants.DEFAULT_OPTION_TCPROFILE, StringComparison.OrdinalIgnoreCase);
    }

    internal static bool TryGetTwincatInstallDirectory([NotNullWhen(true)] out string? tcInstallDir)
    {
        tcInstallDir = RegistryHelper.GetTwincatInstallDirectory();
        if (string.IsNullOrWhiteSpace(tcInstallDir))
        {
            return false;
        }
        return true;
    }

    internal static bool TryGetProfilesDirectory(string tcDir, out string profilesDir)
    {
        profilesDir = Path.Join(tcDir, Constants.TC31_PROFILES_DIRECTORY);
        if (!Directory.Exists(profilesDir))
        {
            return false;
        }
        return true;
    }

    internal static bool TryGetProfileToUse(string profilesDir, [NotNullWhen(true)] out string? profileToUse)
    {
        profileToUse = null;
        var profiles = Directory.GetFiles(profilesDir, Constants.TC31_GLOB_PROFILE);
        if (!profiles.Any())
        {
            return false;
        }

        profileToUse = profiles.OrderByDescending(x => x).First();
        return true;
    }
}
