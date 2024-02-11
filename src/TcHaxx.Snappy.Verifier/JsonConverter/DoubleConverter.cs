using System.Globalization;

namespace TcHaxx.Snappy.Verifier.JsonConverter;

internal class DoubleConverter(ushort precision) : WriteOnlyJsonConverter<double>
{

    public override void Write(VerifyJsonWriter writer, double theDouble) =>
        writer.WriteValue(theDouble.ToString($"F{precision}", CultureInfo.InvariantCulture));
}
