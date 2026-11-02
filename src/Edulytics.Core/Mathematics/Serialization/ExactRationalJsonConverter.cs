using System.Globalization;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Edulytics.Core.Mathematics.Domains;

namespace Edulytics.Core.Mathematics.Serialization;

/// <summary>
/// Stable JSON representation for exact rational values. Numerator and denominator
/// are emitted as invariant strings to preserve arbitrary precision and the reader
/// always reconstructs through the ExactRational constructor so normalization and
/// the non-zero denominator invariant remain enforced.
/// </summary>
public sealed class ExactRationalJsonConverter : JsonConverter<ExactRational>
{
    public override ExactRational Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Exact rational values must be JSON objects.");
        }

        BigInteger? numerator = null;
        BigInteger? denominator = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Invalid exact rational JSON shape.");
            }

            var propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("Unexpected end of exact rational JSON.");
            }

            switch (propertyName?.ToLowerInvariant())
            {
                case "numerator":
                    numerator = ReadBigInteger(ref reader);
                    break;
                case "denominator":
                    denominator = ReadBigInteger(ref reader);
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (numerator is null || denominator is null)
        {
            throw new JsonException("Exact rational JSON must include numerator and denominator.");
        }

        try
        {
            return new ExactRational(numerator.Value, denominator.Value);
        }
        catch (DivideByZeroException ex)
        {
            throw new JsonException("Exact rational denominator cannot be zero.", ex);
        }
    }

    public override void Write(
        Utf8JsonWriter writer,
        ExactRational value,
        JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("numerator", value.Numerator.ToString(CultureInfo.InvariantCulture));
        writer.WriteString("denominator", value.Denominator.ToString(CultureInfo.InvariantCulture));
        writer.WriteEndObject();
    }

    private static BigInteger ReadBigInteger(ref Utf8JsonReader reader)
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

        throw new JsonException("Invalid arbitrary-size integer in exact rational JSON.");
    }
}
