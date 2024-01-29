// See https://aka.ms/new-console-template for more information
using CommandLine;
using System.Reflection;
using TcHaxx.Snappy.CLI;


return await Parser.Default.ParseArguments<InstallOptions, VerfiyOptions>(args)
  .MapResult(
     async (InstallOptions opts) => await CommandInstall.RunAndReturnExitCode(opts),
     async (VerfiyOptions opts) => await CommandVerify.RunAndReturnExitCode(opts),
    errs => Task.FromResult(1));