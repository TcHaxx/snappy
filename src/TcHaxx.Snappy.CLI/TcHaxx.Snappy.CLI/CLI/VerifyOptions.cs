using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcHaxx.Snappy.CLI
{
    [Verb("verify", false, HelpText = "Verify.")]
    internal class VerfiyOptions
    {
        [Option('d', "dir", Default = "./Verified", Required = true, HelpText = "Directory of verified snapshot files.")]
        internal string VerifyDirectory { get; init; } = string.Empty;
    }
}
