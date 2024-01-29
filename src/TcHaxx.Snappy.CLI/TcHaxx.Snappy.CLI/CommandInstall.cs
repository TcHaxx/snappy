using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcHaxx.Snappy.CLI
{
    internal static class CommandInstall
    {

        internal static async Task<int> RunAndReturnExitCode(InstallOptions options)
        {
            await Task.Delay(1000);
            return 0;
        }

    }
}
