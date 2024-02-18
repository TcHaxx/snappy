using TcHaxx.Snappy.Verifier.JsonConverter;

namespace TcHaxx.Snappy.Verifier.Tests.JsonConverter;

public class DoubleConverterTests : VerifyBase
{
    public DoubleConverterTests()
        : base()
    {
    }

    [Theory]
    [InlineData(0, 0.0, -1.00001, 3.00000000000000007, 1234567.987654321)]
    [InlineData(1, 0.0, -1.00001, 3.00000000000000007, 1234567.987654321)]
    [InlineData(2, 0.0, -1.00001, 3.00000000000000007, 1234567.987654321)]
    [InlineData(3, 0.0, -1.00001, 3.00000000000000007, 1234567.987654321)]
    [InlineData(4, 0.0, -1.00001, 3.00000000000000007, 1234567.987654321)]
    [InlineData(5, 0.0, -1.00001, 3.00000000000000007, 1234567.987654321)]
    [InlineData(6, 0.0, -1.00001, 3.00000000000000007, 1234567.987654321, -9.876543321e88)]
    public async Task ShouldUseDoublePrecision(ushort precision, params double[] values)
    {
        var settings = new VerifySettings();
        settings.AddExtraSettings(_ =>
        {
            _.Converters.Add(new DoubleConverter(precision));
        });

        _ = await Verify(values, settings).UseParameters(precision);
    }
}
