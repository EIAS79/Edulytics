using System.Globalization;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Edulytics.Core.Mathematics.Serialization;

/// <summary>
/// Stable JSON representation for arbitrary-size integers used by the Mathematics
/// IR. Values are written as invariant strings so no precision is lost.
/// </summary>
public sealed class BigIntegerJsonConverter : JsonConverter<BigInteger>
{
    public override BigInteger Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String &&
            BigInteger.TryParse(
                reader.GetString(),
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out var parsedString))
        {
            return parsedString;
        }

        if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out var parsedInt64))
        {
            return new BigInteger(parsedInt64);
        }

        throw new JsonException("Invalid arbitrary-size integer representation.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        BigInteger value,
        JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToString(CultureInfo.InvariantCulture));
}
