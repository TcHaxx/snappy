using System.Text.RegularExpressions;
using TcHaxx.Snappy.Common.Verify;

namespace TcHaxx.Snappy.Verifier
{
    internal static class VerificationResultExtension
    {
        internal static VerificationResult ToCompactDiff(this VerificationResult result)
        {

            var match = Regex.Match(result.Diff, "^Compare Result:\n(?<diff>.*)(?<!\n)$", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            if (!match.Success)
            {
                return result;
            }

            result.Diff = match.Groups["diff"].Value;
            return result;
        }
    }
}
