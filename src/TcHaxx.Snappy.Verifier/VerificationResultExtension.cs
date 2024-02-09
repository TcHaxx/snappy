using System.Text.RegularExpressions;
using TcHaxx.Snappy.Common.Verify;

namespace TcHaxx.Snappy.Verifier
{
    internal static class VerificationResultExtension
    {
        internal static VerificationResult ToCompactDiff(this VerificationResult result)
        {
            var matchNew = Regex.Match(result.Diff, "^New:\n\nReceived:(?<diff>.*)(?<!\n)$", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            if (matchNew.Success)
            {
                result.Diff = $"New: {matchNew.Groups["diff"].Value}";
                return result;
            }
            var matchCompareResult = Regex.Match(result.Diff, "^Compare Result:\n(?<diff>.*)(?<!\n)$", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            if (matchCompareResult.Success)
            {
                result.Diff = $"Compare Result:\n{matchCompareResult.Groups["diff"].Value}";
                return result;
            }

            return result;
        }
    }
}
