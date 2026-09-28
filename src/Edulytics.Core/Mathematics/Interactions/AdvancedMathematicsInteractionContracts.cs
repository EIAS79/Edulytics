namespace Edulytics.Core.Mathematics.Interactions;

public enum AdvancedMathematicsInteractionKind
{
    CoordinatePoint = 1,
    CoordinateMultiPoint = 2,
    LineOnGraph = 3,
    CurveChoice = 4,
    GraphFeatureSelection = 5,
    IntervalSelection = 6,
    RegionSelection = 7,
    MatrixEntry = 8,
    VectorEntry = 9,
    ExpressionEntry = 10,
    EquationEntry = 11,
    SetIntervalEntry = 12,
    MultiPartStructured = 13
}

public sealed record AdvancedInteractionContract(
    string Id,
    AdvancedMathematicsInteractionKind Kind,
    int MaximumParts = 1,
    int MaximumEntries = 32,
    bool OrderMatters = true);

public abstract record AdvancedMathematicsResponse(
    AdvancedMathematicsInteractionKind Kind);

public sealed record CoordinatePointResponse(
    decimal X,
    decimal Y)
    : AdvancedMathematicsResponse(
        AdvancedMathematicsInteractionKind.CoordinatePoint);

public sealed record CoordinateMultiPointResponse(
    IReadOnlyList<CoordinatePointResponse> Points)
    : AdvancedMathematicsResponse(
        AdvancedMathematicsInteractionKind.CoordinateMultiPoint);

public sealed record LineOnGraphResponse(
    CoordinatePointResponse First,
    CoordinatePointResponse Second)
    : AdvancedMathematicsResponse(
        AdvancedMathematicsInteractionKind.LineOnGraph);

public sealed record CurveChoiceResponse(
    string ChoiceId)
    : AdvancedMathematicsResponse(
        AdvancedMathematicsInteractionKind.CurveChoice);

public sealed record GraphFeatureSelectionResponse(
    IReadOnlyList<string> FeatureIds)
    : AdvancedMathematicsResponse(
        AdvancedMathematicsInteractionKind.GraphFeatureSelection);

public sealed record IntervalSelectionResponse(
    decimal? Lower,
    decimal? Upper,
    bool LowerClosed,
    bool UpperClosed)
    : AdvancedMathematicsResponse(
        AdvancedMathematicsInteractionKind.IntervalSelection);

public sealed record RegionSelectionResponse(
    IReadOnlyList<string> RegionIds)
    : AdvancedMathematicsResponse(
        AdvancedMathematicsInteractionKind.RegionSelection);

public sealed record MatrixEntryResponse(
    int Rows,
    int Columns,
    IReadOnlyList<string> Values)
    : AdvancedMathematicsResponse(
        AdvancedMathematicsInteractionKind.MatrixEntry);

public sealed record VectorEntryResponse(
    IReadOnlyList<string> Components)
    : AdvancedMathematicsResponse(
        AdvancedMathematicsInteractionKind.VectorEntry);

public sealed record ExpressionEntryResponse(
    string Expression)
    : AdvancedMathematicsResponse(
        AdvancedMathematicsInteractionKind.ExpressionEntry);

public sealed record EquationEntryResponse(
    string Left,
    string Right)
    : AdvancedMathematicsResponse(
        AdvancedMathematicsInteractionKind.EquationEntry);

public sealed record SetIntervalEntryResponse(
    IReadOnlyList<IntervalSelectionResponse> Intervals)
    : AdvancedMathematicsResponse(
        AdvancedMathematicsInteractionKind.SetIntervalEntry);

public sealed record AdvancedResponsePart(
    string PartId,
    AdvancedMathematicsResponse Response);

public sealed record MultiPartStructuredResponse(
    IReadOnlyList<AdvancedResponsePart> Parts)
    : AdvancedMathematicsResponse(
        AdvancedMathematicsInteractionKind.MultiPartStructured);

public static class AdvancedInteractionLimits
{
    public const int MaximumPayloadCharacters = 12_000;
    public const int MaximumTextEntryCharacters = 1_000;
    public const int MaximumChoiceIdCharacters = 120;
    public const int MaximumCoordinatePoints = 32;
    public const int MaximumMatrixDimension = 10;
    public const int MaximumVectorDimension = 12;
    public const int MaximumIntervals = 16;
    public const int MaximumParts = 16;
    public const decimal MaximumCoordinateMagnitude = 1_000_000m;
}
