﻿using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcHaxx.Snappy.CLI.Commands;

internal class CommandInstall : ICommandInstall
{
    private readonly ILogger? _Logger;

    public CommandInstall(ILogger? logger)
    {
        _Logger = logger?.ForContext<CommandInstall>();
    }
    public async Task<int> RunAndReturnExitCode(InstallOptions options)
    {
        await Task.Delay(1000);
        return 0;
    }

}
