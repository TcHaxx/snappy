using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Extensions.Logging;
using System.Reflection;

namespace TcHaxx.Snappy.CLI.Logging;

internal static class MicrosoftILoggerAdapter
{
    internal static Microsoft.Extensions.Logging.ILogger FromSerilog(IServiceProvider sp)
    {
        return new SerilogLoggerFactory(sp.GetService<ILogger>())
                .CreateLogger(Assembly.GetExecutingAssembly().FullName!);
    }
}
