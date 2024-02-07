using System.Runtime.InteropServices;

namespace TcHaxx.Snappy.Common.Verify;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct VerificationResult
{
    public int HResult;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)] public string Diff;
}
