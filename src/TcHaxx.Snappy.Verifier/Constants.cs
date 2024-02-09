using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcHaxx.Snappy.Verifier;

public static class Constants
{
    /// <summary>
    /// Default directory to store all *.verified.txt.
    /// </summary>
    public const string DEFAULT_OPTION_VERIFIED_DIRECTORY = "./TcHaxx.Snappy.Verified";

    /// <summary>
    /// Diff output as compact as possible.
    /// </summary>
    public const bool DEFAULT_OPTION_COMPACT_DIFF = true;
}
