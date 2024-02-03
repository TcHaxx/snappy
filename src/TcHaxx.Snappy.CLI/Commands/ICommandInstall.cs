using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcHaxx.Snappy.CLI.Commands;

internal interface ICommandInstall
{
    Task<int> RunAndReturnExitCode(InstallOptions options);
}
