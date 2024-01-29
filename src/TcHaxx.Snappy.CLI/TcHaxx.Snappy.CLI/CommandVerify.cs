using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcHaxx.Snappy.CLI;

namespace TcHaxx.Snappy.CLI
{
    internal static class CommandVerify
    {

        internal static async Task<int> RunAndReturnExitCode(VerfiyOptions options)
        {
            await Task.Delay(1000);
            return 0;
        }

    }
}
