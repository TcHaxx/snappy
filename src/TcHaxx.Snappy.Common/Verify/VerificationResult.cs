using System.Runtime.InteropServices;
using TcHaxx.Snappy.Common.RPC.Attributes;

namespace TcHaxx.Snappy.Common.Verify;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct VerificationResult
{
    public int HResult;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] public string Diff;
}
