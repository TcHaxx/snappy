using System.Text;

namespace TcHaxx.Snappy.Common.RPC.Attributes;

/// <summary>
/// Defines the expected max string length and encoding.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public class StringAttribute : Attribute
{
    public StringEncoding Encoding { get; private set; }

    public uint Length { get; private set; }

    public StringAttribute(uint length, StringEncoding stringEncoding = StringEncoding.UTF8)
    {
        if (length == 0)
        {
            throw new ArgumentNullException(nameof(length), "Length of 0 is not allowed.");
        }
        if (length > Constants.MAX_STRING_PARAMETER_LENGTH)
        {
            throw new ArgumentOutOfRangeException(nameof(length), length, $"Maximum string-length is {Constants.MAX_STRING_PARAMETER_LENGTH}.");
        }
        Length = length;
        Encoding = stringEncoding;
    }
    public Encoding GetEncoding()
    {
        return Encoding switch
        {
            StringEncoding.UNICODE => System.Text.Encoding.Unicode,
            StringEncoding.ASCII => System.Text.Encoding.ASCII,
            StringEncoding.UTF8 => System.Text.Encoding.UTF8,
            _ => System.Text.Encoding.UTF8
        };
    }
}
