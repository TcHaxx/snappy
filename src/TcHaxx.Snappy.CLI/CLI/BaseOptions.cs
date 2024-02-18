using CommandLine;
using Serilog.Events;

namespace TcHaxx.Snappy.CLI.CLI;

public class BaseOptions
{
    [Option('l', "log-level", Default = LogEventLevel.Information, HelpText = "Minimum LogEventLevel")]
    public LogEventLevel LogEventLevel { get; init; }
}
