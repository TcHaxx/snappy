using System.Reflection;
using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using TcHaxx.Snappy.CLI.CLI;
using TcHaxx.Snappy.CLI.Commands;
using TcHaxx.Snappy.CLI.Logging;
using TcHaxx.Snappy.Common;
using TcHaxx.Snappy.Common.RPC;
using TcHaxx.Snappy.TcADS;
using TcHaxx.Snappy.Verifier;

try
{
    Console.WriteLine(Helper.GetApplicationHeader(Assembly.GetExecutingAssembly()));

    var baseOptions = BaseOptionsParser(args);
    using var host = BuildHost(args, baseOptions);

    var exitCode = await Parser.Default.ParseArguments<InstallOptions, VerifyOptions>(args)
      .MapResult(
        async (InstallOptions options) => await host.Services.GetService<ICommandInstall>()!.RunAndReturnExitCode(options),
        async (VerifyOptions options) => await host.Services.GetService<ICommandVerify>()!.RunAndReturnExitCode(options),
        errs => Task.FromResult(ExitCodes.E_CLIOPTIONS));

    return (int)exitCode;
}
catch (Exception ex)
{
    await Console.Error.WriteLineAsync("❌ Unhandled exception!");
    await Console.Error.WriteLineAsync(ex.Message);
    await Console.Error.WriteLineAsync(ex.StackTrace);
    return (int)ExitCodes.E_EXCEPTION;
}

static IHost BuildHost(string[] args, BaseOptions options)
{
    var logLevelSwitch = new LoggingLevelSwitch(options.LogEventLevel);
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

static BaseOptions BaseOptionsParser(string[] args)
{
    BaseOptions? options = null;
    var baseOptionsParser = new Parser(with =>
    {
        with.EnableDashDash = true;
        with.AutoHelp = true;
        with.AutoVersion = true;
        with.CaseInsensitiveEnumValues = true;
        with.IgnoreUnknownArguments = true;
    });
    var parserResults = baseOptionsParser.ParseArguments<BaseOptions>(args);
    parserResults
        .WithParsed(o => options = o)
        .WithNotParsed(err =>
        {
            var helpText = HelpText.AutoBuild(
                parserResults,
                h => h,
                e => e,
                verbsIndex: true);
            Console.Error.WriteLine(helpText);
        });

    return options ?? new BaseOptions();
}
