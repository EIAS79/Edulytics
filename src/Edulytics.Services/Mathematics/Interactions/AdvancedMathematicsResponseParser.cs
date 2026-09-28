using System.Globalization;
using System.Text.Json;
using Edulytics.Core.Mathematics.Interactions;

namespace Edulytics.Services.Mathematics.Interactions;

public enum AdvancedInteractionParseError
{
    EmptyPayload = 1,
    PayloadTooLarge = 2,
    InvalidJson = 3,
    MissingField = 4,
    InvalidValue = 5,
    LimitExceeded = 6,
    InteractionMismatch = 7
}

public sealed record AdvancedInteractionParseResult(
    AdvancedMathematicsResponse? Response,
    AdvancedInteractionParseError? Error,
    string? Diagnostic)
{
    public bool Succeeded =>
        Response is not null &&
        Error is null;

    public static AdvancedInteractionParseResult Success(
        AdvancedMathematicsResponse response) =>
        new(response, null, null);

    public static AdvancedInteractionParseResult Failure(
        AdvancedInteractionParseError error,
        string diagnostic) =>
        new(null, error, diagnostic);
}

/// <summary>
/// Fail-closed parser for advanced learner interaction payloads.
/// It validates shape and resource bounds only; exact mathematical
/// equivalence remains owned by the mathematics verifier layer.
/// </summary>
public sealed class AdvancedMathematicsResponseParser
{
    public AdvancedInteractionParseResult Parse(
        AdvancedInteractionContract contract,
        string payload)
    {
        ArgumentNullException.ThrowIfNull(contract);

        if (string.IsNullOrWhiteSpace(payload))
        {
            return AdvancedInteractionParseResult.Failure(
                AdvancedInteractionParseError.EmptyPayload,
                "The submitted response is empty.");
        }

        if (payload.Length >
            AdvancedInteractionLimits.MaximumPayloadCharacters)
        {
            return AdvancedInteractionParseResult.Failure(
                AdvancedInteractionParseError.PayloadTooLarge,
                "The submitted response exceeds the bounded payload size.");
        }

        try
        {
            using var document = JsonDocument.Parse(
                payload,
                new JsonDocumentOptions
                {
                    AllowTrailingCommas = false,
                    CommentHandling = JsonCommentHandling.Disallow,
                    MaxDepth = 16
                });

            if (document.RootElement.ValueKind !=
                JsonValueKind.Object)
            {
                return Invalid(
                    "The submitted response must be a JSON object.");
            }

            var response = ParseElement(
                contract.Kind,
                document.RootElement,
                contract,
                depth: 0);

            if (response is null)
            {
                return Invalid(
                    "The submitted response does not match the interaction contract.");
            }

            return AdvancedInteractionParseResult.Success(
                response);
        }
        catch (JsonException)
        {
            return AdvancedInteractionParseResult.Failure(
                AdvancedInteractionParseError.InvalidJson,
                "The submitted response is not valid JSON.");
        }
        catch (InteractionLimitException exception)
        {
            return AdvancedInteractionParseResult.Failure(
                AdvancedInteractionParseError.LimitExceeded,
                exception.Message);
        }
        catch (InteractionShapeException exception)
        {
            return AdvancedInteractionParseResult.Failure(
                AdvancedInteractionParseError.InvalidValue,
                exception.Message);
        }
    }

    private static AdvancedMathematicsResponse? ParseElement(
        AdvancedMathematicsInteractionKind kind,
        JsonElement root,
        AdvancedInteractionContract contract,
        int depth)
    {
        if (depth > 4)
            throw new InteractionLimitException(
                "Nested advanced responses exceed the supported depth.");

        return kind switch
        {
            AdvancedMathematicsInteractionKind.CoordinatePoint =>
                Point(root),

            AdvancedMathematicsInteractionKind.CoordinateMultiPoint =>
                MultiPoint(root),

            AdvancedMathematicsInteractionKind.LineOnGraph =>
                Line(root),

            AdvancedMathematicsInteractionKind.CurveChoice =>
                new CurveChoiceResponse(
                    RequiredBoundedString(
                        root,
                        "choiceId",
                        AdvancedInteractionLimits.MaximumChoiceIdCharacters)),

            AdvancedMathematicsInteractionKind.GraphFeatureSelection =>
                new GraphFeatureSelectionResponse(
                    StringArray(
                        root,
                        "featureIds",
                        contract.MaximumEntries)),

            AdvancedMathematicsInteractionKind.IntervalSelection =>
                Interval(root),

            AdvancedMathematicsInteractionKind.RegionSelection =>
                new RegionSelectionResponse(
                    StringArray(
                        root,
                        "regionIds",
                        contract.MaximumEntries)),

            AdvancedMathematicsInteractionKind.MatrixEntry =>
                Matrix(root),

            AdvancedMathematicsInteractionKind.VectorEntry =>
                Vector(root),

            AdvancedMathematicsInteractionKind.ExpressionEntry =>
                new ExpressionEntryResponse(
                    RequiredBoundedString(
                        root,
                        "expression",
                        AdvancedInteractionLimits.MaximumTextEntryCharacters)),

            AdvancedMathematicsInteractionKind.EquationEntry =>
                new EquationEntryResponse(
                    RequiredBoundedString(
                        root,
                        "left",
                        AdvancedInteractionLimits.MaximumTextEntryCharacters),
                    RequiredBoundedString(
                        root,
                        "right",
                        AdvancedInteractionLimits.MaximumTextEntryCharacters)),

            AdvancedMathematicsInteractionKind.SetIntervalEntry =>
                IntervalSet(root),

            AdvancedMathematicsInteractionKind.MultiPartStructured =>
                MultiPart(
                    root,
                    contract,
                    depth),

            _ => null
        };
    }

    private static CoordinatePointResponse Point(
        JsonElement root)
    {
        var x = RequiredDecimal(root, "x");
        var y = RequiredDecimal(root, "y");
        ValidateCoordinate(x);
        ValidateCoordinate(y);
        return new CoordinatePointResponse(x, y);
    }

    private static CoordinateMultiPointResponse MultiPoint(
        JsonElement root)
    {
        var rows = RequiredArray(root, "points");
        if (rows.GetArrayLength() >
            AdvancedInteractionLimits.MaximumCoordinatePoints)
        {
            throw new InteractionLimitException(
                "Too many coordinate points were submitted.");
        }

        var points = rows
            .EnumerateArray()
            .Select(Point)
            .ToArray();

        if (points.Length == 0)
            throw new InteractionShapeException(
                "At least one coordinate point is required.");

        return new CoordinateMultiPointResponse(points);
    }

    private static LineOnGraphResponse Line(
        JsonElement root)
    {
        var first = RequiredObject(root, "first");
        var second = RequiredObject(root, "second");

        var a = Point(first);
        var b = Point(second);

        if (a == b)
            throw new InteractionShapeException(
                "A line requires two distinct points.");

        return new LineOnGraphResponse(a, b);
    }

    private static IntervalSelectionResponse Interval(
        JsonElement root)
    {
        var lower = OptionalDecimal(root, "lower");
        var upper = OptionalDecimal(root, "upper");
        var lowerClosed = OptionalBoolean(
            root,
            "lowerClosed");
        var upperClosed = OptionalBoolean(
            root,
            "upperClosed");

        if (lower.HasValue)
            ValidateCoordinate(lower.Value);
        if (upper.HasValue)
            ValidateCoordinate(upper.Value);

        if (lower.HasValue &&
            upper.HasValue &&
            lower.Value > upper.Value)
        {
            throw new InteractionShapeException(
                "Interval lower bound cannot exceed its upper bound.");
        }

        if (!lower.HasValue && lowerClosed)
            throw new InteractionShapeException(
                "An infinite lower interval boundary cannot be closed.");

        if (!upper.HasValue && upperClosed)
            throw new InteractionShapeException(
                "An infinite upper interval boundary cannot be closed.");

        return new IntervalSelectionResponse(
            lower,
            upper,
            lowerClosed,
            upperClosed);
    }

    private static MatrixEntryResponse Matrix(
        JsonElement root)
    {
        var rows = RequiredInt(root, "rows");
        var columns = RequiredInt(root, "columns");

        if (rows is < 1 or >
                AdvancedInteractionLimits.MaximumMatrixDimension ||
            columns is < 1 or >
                AdvancedInteractionLimits.MaximumMatrixDimension)
        {
            throw new InteractionLimitException(
                "Matrix dimensions exceed the supported bounds.");
        }

        var values = RequiredArray(root, "values");
        if (values.GetArrayLength() != rows * columns)
        {
            throw new InteractionShapeException(
                "Matrix value count does not match its dimensions.");
        }

        var entries = values
            .EnumerateArray()
            .Select(EntryString)
            .ToArray();

        return new MatrixEntryResponse(
            rows,
            columns,
            entries);
    }

    private static VectorEntryResponse Vector(
        JsonElement root)
    {
        var values = RequiredArray(root, "components");
        if (values.GetArrayLength() is < 1 or >
            AdvancedInteractionLimits.MaximumVectorDimension)
        {
            throw new InteractionLimitException(
                "Vector dimension exceeds the supported bounds.");
        }

        return new VectorEntryResponse(
            values
                .EnumerateArray()
                .Select(EntryString)
                .ToArray());
    }

    private static SetIntervalEntryResponse IntervalSet(
        JsonElement root)
    {
        var values = RequiredArray(root, "intervals");
        if (values.GetArrayLength() is < 1 or >
            AdvancedInteractionLimits.MaximumIntervals)
        {
            throw new InteractionLimitException(
                "Interval-set size exceeds the supported bounds.");
        }

        return new SetIntervalEntryResponse(
            values
                .EnumerateArray()
                .Select(Interval)
                .ToArray());
    }

    private static MultiPartStructuredResponse MultiPart(
        JsonElement root,
        AdvancedInteractionContract contract,
        int depth)
    {
        var values = RequiredArray(root, "parts");
        var maximumParts = Math.Min(
            AdvancedInteractionLimits.MaximumParts,
            Math.Max(1, contract.MaximumParts));

        if (values.GetArrayLength() is < 1 ||
            values.GetArrayLength() > maximumParts)
        {
            throw new InteractionLimitException(
                "Multi-part response exceeds the configured part limit.");
        }

        var parts = new List<AdvancedResponsePart>();
        var ids = new HashSet<string>(
            StringComparer.Ordinal);

        foreach (var row in values.EnumerateArray())
        {
            var id = RequiredBoundedString(
                row,
                "id",
                AdvancedInteractionLimits.MaximumChoiceIdCharacters);

            if (!ids.Add(id))
                throw new InteractionShapeException(
                    "Multi-part response contains duplicate part ids.");

            var kindText = RequiredBoundedString(
                row,
                "kind",
                80);

            if (!Enum.TryParse<
                    AdvancedMathematicsInteractionKind>(
                    kindText,
                    ignoreCase: true,
                    out var partKind) ||
                partKind ==
                    AdvancedMathematicsInteractionKind.MultiPartStructured)
            {
                throw new InteractionShapeException(
                    "Multi-part response contains an unsupported part kind.");
            }

            var responseNode =
                RequiredObject(row, "response");

            var response = ParseElement(
                partKind,
                responseNode,
                contract,
                depth + 1)
                ?? throw new InteractionShapeException(
                    "Multi-part response contains an invalid part.");

            parts.Add(
                new AdvancedResponsePart(
                    id,
                    response));
        }

        return new MultiPartStructuredResponse(parts);
    }

    private static IReadOnlyList<string> StringArray(
        JsonElement root,
        string property,
        int configuredMaximum)
    {
        var values = RequiredArray(root, property);
        var maximum = Math.Max(
            1,
            Math.Min(
                configuredMaximum,
                AdvancedInteractionLimits.MaximumCoordinatePoints));

        if (values.GetArrayLength() is < 1 ||
            values.GetArrayLength() > maximum)
        {
            throw new InteractionLimitException(
                $"{property} exceeds the supported item limit.");
        }

        var result = values
            .EnumerateArray()
            .Select(element =>
                BoundedString(
                    element,
                    AdvancedInteractionLimits.MaximumChoiceIdCharacters))
            .ToArray();

        if (result.Distinct(
                StringComparer.Ordinal).Count() != result.Length)
        {
            throw new InteractionShapeException(
                $"{property} contains duplicate identifiers.");
        }

        return result;
    }

    private static string EntryString(
        JsonElement element) =>
        BoundedString(
            element,
            AdvancedInteractionLimits.MaximumTextEntryCharacters);

    private static string RequiredBoundedString(
        JsonElement root,
        string property,
        int maximumLength)
    {
        if (!root.TryGetProperty(
                property,
                out var element))
        {
            throw new InteractionShapeException(
                $"Missing required field '{property}'.");
        }

        return BoundedString(
            element,
            maximumLength);
    }

    private static string BoundedString(
        JsonElement element,
        int maximumLength)
    {
        if (element.ValueKind != JsonValueKind.String)
            throw new InteractionShapeException(
                "Expected a text value.");

        var value = element.GetString()?.Trim()
            ?? string.Empty;

        if (value.Length is < 1 ||
            value.Length > maximumLength)
        {
            throw new InteractionLimitException(
                "Text response exceeds the supported bounds.");
        }

        return value;
    }

    private static decimal RequiredDecimal(
        JsonElement root,
        string property)
    {
        if (!root.TryGetProperty(property, out var element))
            throw new InteractionShapeException(
                $"Missing required field '{property}'.");

        return ParseDecimal(element);
    }

    private static decimal? OptionalDecimal(
        JsonElement root,
        string property)
    {
        if (!root.TryGetProperty(
                property,
                out var element) ||
            element.ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        return ParseDecimal(element);
    }

    private static decimal ParseDecimal(
        JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Number &&
            element.TryGetDecimal(out var numeric))
        {
            return numeric;
        }

        if (element.ValueKind == JsonValueKind.String &&
            decimal.TryParse(
                element.GetString(),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out numeric))
        {
            return numeric;
        }

        throw new InteractionShapeException(
            "Expected a bounded decimal value.");
    }

    private static int RequiredInt(
        JsonElement root,
        string property)
    {
        if (!root.TryGetProperty(
                property,
                out var element) ||
            element.ValueKind != JsonValueKind.Number ||
            !element.TryGetInt32(out var value))
        {
            throw new InteractionShapeException(
                $"Missing or invalid integer field '{property}'.");
        }

        return value;
    }

    private static bool OptionalBoolean(
        JsonElement root,
        string property)
    {
        if (!root.TryGetProperty(
                property,
                out var element))
        {
            return false;
        }

        if (element.ValueKind is not (
            JsonValueKind.True or
            JsonValueKind.False))
        {
            throw new InteractionShapeException(
                $"Field '{property}' must be boolean.");
        }

        return element.GetBoolean();
    }

    private static JsonElement RequiredArray(
        JsonElement root,
        string property)
    {
        if (!root.TryGetProperty(
                property,
                out var element) ||
            element.ValueKind != JsonValueKind.Array)
        {
            throw new InteractionShapeException(
                $"Missing or invalid array field '{property}'.");
        }

        return element;
    }

    private static JsonElement RequiredObject(
        JsonElement root,
        string property)
    {
        if (!root.TryGetProperty(
                property,
                out var element) ||
            element.ValueKind != JsonValueKind.Object)
        {
            throw new InteractionShapeException(
                $"Missing or invalid object field '{property}'.");
        }

        return element;
    }

    private static void ValidateCoordinate(decimal value)
    {
        if (decimal.Abs(value) >
            AdvancedInteractionLimits.MaximumCoordinateMagnitude)
        {
            throw new InteractionLimitException(
                "Coordinate magnitude exceeds the supported bound.");
        }
    }

    private static AdvancedInteractionParseResult Invalid(
        string diagnostic) =>
        AdvancedInteractionParseResult.Failure(
            AdvancedInteractionParseError.InvalidValue,
            diagnostic);

    private sealed class InteractionShapeException(
        string message)
        : Exception(message);

    private sealed class InteractionLimitException(
        string message)
        : Exception(message);
}
