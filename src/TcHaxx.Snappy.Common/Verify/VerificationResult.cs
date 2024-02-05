using System.Runtime.InteropServices;
using TcHaxx.Snappy.Common.RPC.Attributes;

namespace TcHaxx.Snappy.Common.Verify;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct VerificationResult
{
    public int HResult;
    [String(256, StringEncoding.UNICODE)] public string Diff;
}
