using System.Diagnostics;
using Serilog;
using TcHaxx.Snappy.Common;

namespace TcHaxx.Snappy.CLI.Installer;
internal static class RepToolProcess
{
    internal static async Task<ExitCodes> RunRepToolAsync(TwincatProfile tcProfile, DirectoryInfo sourceDirectory, ILogger? logger)
    {
        var tcDir = RegistryHelper.GetTwincatInstallDirectory();
        if (string.IsNullOrWhiteSpace(tcDir))
        {
            logger?.Error("Couldn't read TwinCAT installation directory from Registry.");
            return ExitCodes.E_ERROR;
        }

        var repToolExe = Path.Join(tcDir, Constants.TC31_REPTOOL_EXE);
        if (!File.Exists(repToolExe))
        {
            throw new FileNotFoundException("RepTool.exe doesn't exist.", repToolExe);
        }

        try
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = repToolExe,
                Arguments = $"--profile='{tcProfile.Profile}' --installLibsRecursNoOverwrite \"{sourceDirectory.FullName}\"",
                UseShellExecute = false,
                RedirectStandardInput = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            process.OutputDataReceived += (sender, args) =>
            {
                if (string.IsNullOrEmpty(args.Data))
                {
                    return;
                }
                logger?.Information("RepTool.exe: {StandardOutput}", args.Data ?? string.Empty);
            };
            process.ErrorDataReceived += (sender, args) =>
            {
                if (string.IsNullOrEmpty(args.Data))
                {
                    return;
                }

                logger?.Error("RepTool.exe: {StandardError}", args.Data ?? string.Empty);
            };

            logger?.Information("Installing TwinCAT libraries ...");

            process.Start();
            var stdout = await process.StandardOutput.ReadToEndAsync();
            logger?.Information("RepTool.exe: {StandardOutput}", stdout ?? string.Empty);

            process.WaitForExit(Constants.REPTOOL_EXE_TIMEOUT_MS);
            return (ExitCodes)process.ExitCode;
        }
        catch (Exception ex)
        {
            logger?.Fatal(ex, "Couldn't install TwinCAT libraries.");
            return ExitCodes.E_EXCEPTION;
        }
    }
}
