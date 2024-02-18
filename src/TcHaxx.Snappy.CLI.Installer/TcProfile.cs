using Serilog;
using TcHaxx.Snappy.CLI.Installer.Options;

namespace TcHaxx.Snappy.CLI.Installer;
internal static class TcProfile
{
    internal static TwincatProfile? GetTwinCatProfile(IInstallerOptions options, ILogger? logger)
    {
        if (!options.TcProfile.Equals(Constants.DEFAULT_OPTION_TCPROFILE, StringComparison.OrdinalIgnoreCase))
        {
            logger?.Information("Using provided TwinCAT profile \"{TwinCatProfile}\"", options.TcProfile);
            return new TwincatProfile(options.TcProfile);
        }

        var tcDir = RegistryHelper.GetTwincatInstallDirectory();
        if (string.IsNullOrWhiteSpace(tcDir))
        {
            logger?.Error("Couldn't read TwinCAT installation directory from Registry.");
            return default;
        }

        var profilesDir = Path.Join(tcDir, Constants.TC31_PROFILES_DIRECTORY);
        if (!Directory.Exists(profilesDir))
        {
            logger?.Error("Directory \"{TwinCatProfileDirectory}\" doesn't exist.", profilesDir);
            return default;
        }

        var profiles = Directory.GetFiles(profilesDir, Constants.TC31_GLOB_PROFILE);
        if (!profiles.Any())
        {
            logger?.Error("Couldn't find any profiles \"{TwinCatProfileGlob}\" in \"{TwinCatProfileDirectory}\".",
                Constants.TC31_GLOB_PROFILE, profilesDir);
        }

        var profileToUse = profiles.OrderByDescending(x => x).First();
        logger?.Information("Using TwinCAT profile \"{TwinCatProfile}\"", profileToUse);
        return new TwincatProfile(Path.GetFileNameWithoutExtension(profileToUse));
    }
}

