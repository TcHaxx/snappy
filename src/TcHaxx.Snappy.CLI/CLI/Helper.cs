using System.Reflection;
using System.Text;

namespace TcHaxx.Snappy.CLI;

/// <summary>
/// Some helper methods/functions and extension methods.
/// </summary>
internal static class Helper
{
    /// <summary>
    /// Centers a <see cref="string"/>.
    /// </summary>
    /// <param name="this"></param>
    /// <returns>centered string</returns>
    internal static string Center(this string @this)
    {

        if (string.IsNullOrEmpty(@this))
        {
            return string.Empty;
        }
        var width = Console.BufferWidth;
        return @this.PadLeft(((width - @this.Length) / 2)
                              + @this.Length)
                              .PadRight(width);
    }

    /// <summary>
    /// Prints the application header based on <see cref="Assembly"/>-attributes as <see cref="string"/>.
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    internal static string GetApplicationHeader(Assembly assembly)
    {
        var sb = new StringBuilder(1000);

        var version = assembly.GetName().Version;
        var copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright ?? Constants.DEFAULT_COPYRIGHT;

        var repoUrl = assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
                              .FirstOrDefault(x => x.Key.Equals("RepositoryUrl"))?.Value ?? string.Empty;

        var delimiter = new string(Constants.CLI_DELIMITER, Console.BufferWidth);
        sb.Append($"{delimiter}\n");
        sb.Append($"{assembly.GetName().Name} V{version}".Center() + "\n");
        sb.Append($"{copyright.Center()}\n");
        if (!string.IsNullOrWhiteSpace(repoUrl))
        {
            sb.Append($"{repoUrl.Center()}\n");
        }

        sb.Append($"{delimiter}\n");

        return sb.ToString();
    }
}
