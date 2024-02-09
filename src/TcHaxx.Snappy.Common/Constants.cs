using System.Text;

namespace TcHaxx.Snappy.Common;

public class Constants
{
    public const int DEFAULT_STRING_PARAMETER_LENGTH = 128;
    public const int DEFAULT_JSON_PARAMETER_LENGTH = 16384;
    public const int MAX_STRING_PARAMETER_LENGTH = ushort.MaxValue;
    public static readonly Encoding DEFAULT_STRING_ENCODING = Encoding.UTF8;
}
