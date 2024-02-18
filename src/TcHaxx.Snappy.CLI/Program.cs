using System.Reflection;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using TcHaxx.Snappy.CLI.CLI;
using TcHaxx.Snappy.CLI.Commands;
using TcHaxx.Snappy.CLI.Logging;
using TcHaxx.Snappy.Common.RPC;
using TcHaxx.Snappy.TcADS;
using TcHaxx.Snappy.Verifier;

try
{
    Console.WriteLine(Helper.GetApplicationHeader(Assembly.GetExecutingAssembly()));

    using var host = BuildHost(args);

    return await Parser.Default.ParseArguments<InstallOptions, VerifyOptions>(args)
      .MapResult(
        async (InstallOptions options) => await host.Services.GetService<ICommandInstall>()!.RunAndReturnExitCode(options),
        async (VerifyOptions options) => await host.Services.GetService<ICommandVerify>()!.RunAndReturnExitCode(options),
        errs => Task.FromResult((int)ExitCodes.E_CLIOPTIONS)
        );
}
catch (Exception ex)
{
    Console.Error.WriteLine("❌ Unhandled exception!");
    Console.Error.WriteLine(ex.Message);
    Console.Error.WriteLine(ex.StackTrace);
    return (int)ExitCodes.E_EXCEPTION;
}

static IHost BuildHost(string[] args)
{
    var baseOptionsParser = new Parser(with =>
    {
        with.EnableDashDash = true;
        with.AutoHelp = true;
        with.AutoVersion = true;
        with.CaseInsensitiveEnumValues = true;
        with.IgnoreUnknownArguments = true;
    });
    var baseOptions = baseOptionsParser.ParseArguments<BaseOptions>(args);

    var logLevelSwitch = new LoggingLevelSwitch(baseOptions.Value.LogEventLevel);
    var host = new HostBuilder()
        .ConfigureDefaults(args)
        .ConfigureServices(services => services
                .AddSingleton(serviceProvider => MicrosoftILoggerAdapter.FromSerilog(serviceProvider))
                .AddSingleton<ICommandInstall, CommandInstall>()
                .AddSingleton<ICommandVerify, CommandVerify>()
                .AddSingleton<IRpcMethodDescriptor, RpcMethodDescriptor>()
                .AddTransient<IVerifyService, VerifyService>()
                .AddTransient<IVerifyService, ExampleAnotherVerifyService>()
        .AddSingleton<ISymbolicServerFactory, SymbolicServerFactory>())
        .UseSerilog((hostingContext, services, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .MinimumLevel.ControlledBy(logLevelSwitch))
        .Build();
    return host;
}
