using System.Runtime.InteropServices;

namespace TcHaxx.Snappy.Common.Verify;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct VerificationResult
{
    public int HResult;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.DEFAULT_VERIFICATION_RESULT_LENGTH)]
    public string Diff;
}
