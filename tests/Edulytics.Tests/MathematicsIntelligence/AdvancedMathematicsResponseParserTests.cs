using Edulytics.Core.Mathematics.Interactions;
using Edulytics.Services.Mathematics.Interactions;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class AdvancedMathematicsResponseParserTests
{
    private readonly AdvancedMathematicsResponseParser parser = new();

    [Fact]
    public void ParsesCoordinatePoint()
    {
        var result = parser.Parse(
            Contract(
                AdvancedMathematicsInteractionKind.CoordinatePoint),
            """{"x":"3/2","y":-2}""");

        Assert.False(result.Succeeded);
        Assert.Equal(
            AdvancedInteractionParseError.InvalidValue,
            result.Error);

        var numeric = parser.Parse(
            Contract(
                AdvancedMathematicsInteractionKind.CoordinatePoint),
            """{"x":1.5,"y":-2}""");

        Assert.True(numeric.Succeeded);
        var point = Assert.IsType<CoordinatePointResponse>(
            numeric.Response);
        Assert.Equal(1.5m, point.X);
        Assert.Equal(-2m, point.Y);
    }

    [Fact]
    public void ParsesMatrixWithBoundedDimensions()
    {
        var result = parser.Parse(
            Contract(
                AdvancedMathematicsInteractionKind.MatrixEntry),
            """
            {
              "rows": 2,
              "columns": 2,
              "values": ["1", "2", "3", "4"]
            }
            """);

        Assert.True(result.Succeeded);
        var matrix = Assert.IsType<MatrixEntryResponse>(
            result.Response);
        Assert.Equal(2, matrix.Rows);
        Assert.Equal(2, matrix.Columns);
        Assert.Equal(["1", "2", "3", "4"], matrix.Values);
    }

    [Fact]
    public void RejectsMatrixValueCountMismatch()
    {
        var result = parser.Parse(
            Contract(
                AdvancedMathematicsInteractionKind.MatrixEntry),
            """
            {
              "rows": 2,
              "columns": 2,
              "values": ["1", "2", "3"]
            }
            """);

        Assert.False(result.Succeeded);
        Assert.Equal(
            AdvancedInteractionParseError.InvalidValue,
            result.Error);
    }

    [Fact]
    public void ParsesOpenInfiniteIntervalSafely()
    {
        var result = parser.Parse(
            Contract(
                AdvancedMathematicsInteractionKind.IntervalSelection),
            """
            {
              "lower": null,
              "upper": 4,
              "lowerClosed": false,
              "upperClosed": true
            }
            """);

        Assert.True(result.Succeeded);
        var interval = Assert.IsType<IntervalSelectionResponse>(
            result.Response);
        Assert.Null(interval.Lower);
        Assert.Equal(4m, interval.Upper);
        Assert.False(interval.LowerClosed);
        Assert.True(interval.UpperClosed);
    }

    [Fact]
    public void RejectsClosedInfiniteBoundary()
    {
        var result = parser.Parse(
            Contract(
                AdvancedMathematicsInteractionKind.IntervalSelection),
            """
            {
              "lower": null,
              "upper": 4,
              "lowerClosed": true,
              "upperClosed": true
            }
            """);

        Assert.False(result.Succeeded);
        Assert.Equal(
            AdvancedInteractionParseError.InvalidValue,
            result.Error);
    }

    [Fact]
    public void MultiPartResponseIsTypedAndRejectsNestedMultiPart()
    {
        var result = parser.Parse(
            new AdvancedInteractionContract(
                "calculus.multistep",
                AdvancedMathematicsInteractionKind.MultiPartStructured,
                MaximumParts: 4),
            """
            {
              "parts": [
                {
                  "id": "a",
                  "kind": "ExpressionEntry",
                  "response": { "expression": "2x + 3" }
                },
                {
                  "id": "b",
                  "kind": "CoordinatePoint",
                  "response": { "x": 1, "y": 5 }
                }
              ]
            }
            """);

        Assert.True(result.Succeeded);
        var multipart =
            Assert.IsType<MultiPartStructuredResponse>(
                result.Response);
        Assert.Equal(2, multipart.Parts.Count);
        Assert.IsType<ExpressionEntryResponse>(
            multipart.Parts[0].Response);
        Assert.IsType<CoordinatePointResponse>(
            multipart.Parts[1].Response);
    }

    [Fact]
    public void RejectsOversizedPayloadBeforeJsonParsing()
    {
        var payload = new string(
            'x',
            AdvancedInteractionLimits.MaximumPayloadCharacters + 1);

        var result = parser.Parse(
            Contract(
                AdvancedMathematicsInteractionKind.ExpressionEntry),
            payload);

        Assert.False(result.Succeeded);
        Assert.Equal(
            AdvancedInteractionParseError.PayloadTooLarge,
            result.Error);
    }

    [Fact]
    public void RejectsDuplicateFeatureIds()
    {
        var result = parser.Parse(
            Contract(
                AdvancedMathematicsInteractionKind.GraphFeatureSelection),
            """
            {
              "featureIds": ["turning-point", "turning-point"]
            }
            """);

        Assert.False(result.Succeeded);
        Assert.Equal(
            AdvancedInteractionParseError.InvalidValue,
            result.Error);
    }

    private static AdvancedInteractionContract Contract(
        AdvancedMathematicsInteractionKind kind) =>
        new(
            "test",
            kind,
            MaximumParts: 8,
            MaximumEntries: 16);
}
