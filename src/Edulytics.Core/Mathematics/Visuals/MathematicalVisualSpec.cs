namespace Edulytics.Core.Mathematics.Visuals;

public enum MathematicalVisualKind
{
    CoordinatePlane = 1,
    FunctionGraph = 2,
    MultiFunctionGraph = 3,
    GeometryDiagram = 4,
    VectorDiagram = 5,
    MatrixDisplay = 6,
    ArgandDiagram = 7,
    RegionOfIntegration = 8,
    StatisticalPlot = 9
}

public enum MathematicalCurveKind
{
    Polynomial = 1,
    Sine = 2,
    Cosine = 3,
    Tangent = 4
}

public enum StatisticalPlotKind
{
    Scatter = 1,
    Line = 2,
    Bar = 3
}

public sealed record MathematicalVisualAccessibility(
    string Title,
    string Description);

public sealed record MathematicalAxisWindow(
    decimal XMin,
    decimal XMax,
    decimal YMin,
    decimal YMax,
    decimal XTick,
    decimal YTick);

public sealed record MathematicalPoint(
    decimal X,
    decimal Y,
    string? Label = null);

public sealed record MathematicalSegment(
    MathematicalPoint Start,
    MathematicalPoint End,
    string? Label = null,
    bool Dashed = false);

/// <summary>
/// Typed function contract for deterministic rendering only.
/// Polynomial parameters are coefficients in ascending power order.
/// Trigonometric parameters are [amplitude, angularFrequency, phase, verticalShift].
/// Mathematical solving/verification remains owned by the exact Mathematics engines.
/// </summary>
public sealed record MathematicalCurveSpec(
    string Id,
    MathematicalCurveKind Kind,
    IReadOnlyList<decimal> Parameters,
    string? Label = null);

public abstract record MathematicalVisualSpec(
    MathematicalVisualKind Kind,
    MathematicalVisualAccessibility Accessibility);

public sealed record CoordinatePlaneVisualSpec(
    MathematicalAxisWindow Window,
    IReadOnlyList<MathematicalPoint> Points,
    IReadOnlyList<MathematicalSegment> Segments,
    MathematicalVisualAccessibility Accessibility)
    : MathematicalVisualSpec(
        MathematicalVisualKind.CoordinatePlane,
        Accessibility);

public sealed record FunctionGraphVisualSpec(
    MathematicalAxisWindow Window,
    MathematicalCurveSpec Curve,
    IReadOnlyList<MathematicalPoint> HighlightedPoints,
    MathematicalVisualAccessibility Accessibility)
    : MathematicalVisualSpec(
        MathematicalVisualKind.FunctionGraph,
        Accessibility);

public sealed record MultiFunctionGraphVisualSpec(
    MathematicalAxisWindow Window,
    IReadOnlyList<MathematicalCurveSpec> Curves,
    IReadOnlyList<MathematicalPoint> HighlightedPoints,
    MathematicalVisualAccessibility Accessibility)
    : MathematicalVisualSpec(
        MathematicalVisualKind.MultiFunctionGraph,
        Accessibility);

public sealed record GeometryDiagramVisualSpec(
    IReadOnlyList<MathematicalPoint> Points,
    IReadOnlyList<MathematicalSegment> Segments,
    IReadOnlyList<IReadOnlyList<int>> Polygons,
    MathematicalVisualAccessibility Accessibility)
    : MathematicalVisualSpec(
        MathematicalVisualKind.GeometryDiagram,
        Accessibility);

public sealed record MathematicalVectorArrow(
    MathematicalPoint Start,
    MathematicalPoint End,
    string? Label = null);

public sealed record VectorDiagramVisualSpec(
    MathematicalAxisWindow Window,
    IReadOnlyList<MathematicalVectorArrow> Vectors,
    IReadOnlyList<MathematicalPoint> Points,
    MathematicalVisualAccessibility Accessibility)
    : MathematicalVisualSpec(
        MathematicalVisualKind.VectorDiagram,
        Accessibility);

public sealed record MathematicalMatrix(
    int Rows,
    int Columns,
    IReadOnlyList<string> Values,
    string? Label = null);

public sealed record MatrixDisplayVisualSpec(
    IReadOnlyList<MathematicalMatrix> Matrices,
    MathematicalVisualAccessibility Accessibility)
    : MathematicalVisualSpec(
        MathematicalVisualKind.MatrixDisplay,
        Accessibility);

public sealed record MathematicalComplexPoint(
    decimal Real,
    decimal Imaginary,
    string? Label = null);

public sealed record ArgandDiagramVisualSpec(
    MathematicalAxisWindow Window,
    IReadOnlyList<MathematicalComplexPoint> Points,
    MathematicalVisualAccessibility Accessibility)
    : MathematicalVisualSpec(
        MathematicalVisualKind.ArgandDiagram,
        Accessibility);

public sealed record RegionOfIntegrationVisualSpec(
    MathematicalAxisWindow Window,
    MathematicalCurveSpec FirstCurve,
    MathematicalCurveSpec SecondCurve,
    decimal XFrom,
    decimal XTo,
    IReadOnlyList<MathematicalPoint> Intersections,
    MathematicalVisualAccessibility Accessibility)
    : MathematicalVisualSpec(
        MathematicalVisualKind.RegionOfIntegration,
        Accessibility);

public sealed record StatisticalDataPoint(
    decimal X,
    decimal Y,
    string? Label = null);

public sealed record StatisticalPlotVisualSpec(
    MathematicalAxisWindow Window,
    StatisticalPlotKind PlotKind,
    IReadOnlyList<StatisticalDataPoint> Points,
    MathematicalVisualAccessibility Accessibility)
    : MathematicalVisualSpec(
        MathematicalVisualKind.StatisticalPlot,
        Accessibility);

public static class MathematicalVisualSpecValidator
{
    public const int MaximumCurves = 8;
    public const int MaximumPoints = 128;
    public const int MaximumSegments = 128;
    public const int MaximumMatrixDimension = 10;
    public const int MaximumMatrices = 4;
    public const int MaximumLabelLength = 160;
    public const decimal MaximumAxisSpan = 10_000m;

    public static void Validate(MathematicalVisualSpec spec)
    {
        ArgumentNullException.ThrowIfNull(spec);

        ValidateAccessibility(spec.Accessibility);

        switch (spec)
        {
            case CoordinatePlaneVisualSpec coordinate:
                ValidateWindow(coordinate.Window);
                ValidatePoints(coordinate.Points);
                ValidateSegments(coordinate.Segments);
                break;

            case FunctionGraphVisualSpec function:
                ValidateWindow(function.Window);
                ValidateCurve(function.Curve);
                ValidatePoints(function.HighlightedPoints);
                break;

            case MultiFunctionGraphVisualSpec multi:
                ValidateWindow(multi.Window);
                if (multi.Curves.Count is < 1 or > MaximumCurves)
                    throw new ArgumentOutOfRangeException(nameof(multi.Curves));
                foreach (var curve in multi.Curves)
                    ValidateCurve(curve);
                ValidatePoints(multi.HighlightedPoints);
                break;

            case GeometryDiagramVisualSpec geometry:
                ValidatePoints(geometry.Points);
                ValidateSegments(geometry.Segments);
                if (geometry.Polygons.Count > MaximumSegments)
                    throw new ArgumentOutOfRangeException(nameof(geometry.Polygons));
                foreach (var polygon in geometry.Polygons)
                {
                    if (polygon.Count is < 3 or > MaximumPoints)
                        throw new ArgumentOutOfRangeException(nameof(geometry.Polygons));
                    foreach (var index in polygon)
                    {
                        if (index < 0 || index >= geometry.Points.Count)
                            throw new ArgumentOutOfRangeException(nameof(geometry.Polygons));
                    }
                }
                break;

            case VectorDiagramVisualSpec vectors:
                ValidateWindow(vectors.Window);
                if (vectors.Vectors.Count > MaximumSegments)
                    throw new ArgumentOutOfRangeException(nameof(vectors.Vectors));
                foreach (var vector in vectors.Vectors)
                {
                    ValidatePoint(vector.Start);
                    ValidatePoint(vector.End);
                    ValidateLabel(vector.Label);
                }
                ValidatePoints(vectors.Points);
                break;

            case MatrixDisplayVisualSpec matrixDisplay:
                if (matrixDisplay.Matrices.Count is < 1 or > MaximumMatrices)
                    throw new ArgumentOutOfRangeException(nameof(matrixDisplay.Matrices));
                foreach (var matrix in matrixDisplay.Matrices)
                    ValidateMatrix(matrix);
                break;

            case ArgandDiagramVisualSpec argand:
                ValidateWindow(argand.Window);
                if (argand.Points.Count > MaximumPoints)
                    throw new ArgumentOutOfRangeException(nameof(argand.Points));
                foreach (var point in argand.Points)
                {
                    ValidateCoordinate(point.Real, nameof(point.Real));
                    ValidateCoordinate(point.Imaginary, nameof(point.Imaginary));
                    ValidateLabel(point.Label);
                }
                break;

            case RegionOfIntegrationVisualSpec region:
                ValidateWindow(region.Window);
                ValidateCurve(region.FirstCurve);
                ValidateCurve(region.SecondCurve);
                ValidateCoordinate(region.XFrom, nameof(region.XFrom));
                ValidateCoordinate(region.XTo, nameof(region.XTo));
                if (region.XFrom >= region.XTo)
                    throw new ArgumentOutOfRangeException(nameof(region.XTo));
                if (region.XFrom < region.Window.XMin ||
                    region.XTo > region.Window.XMax)
                    throw new ArgumentOutOfRangeException(nameof(region.XFrom));
                ValidatePoints(region.Intersections);
                break;

            case StatisticalPlotVisualSpec statistics:
                ValidateWindow(statistics.Window);
                if (statistics.Points.Count is < 1 or > MaximumPoints)
                    throw new ArgumentOutOfRangeException(nameof(statistics.Points));
                foreach (var point in statistics.Points)
                {
                    ValidateCoordinate(point.X, nameof(point.X));
                    ValidateCoordinate(point.Y, nameof(point.Y));
                    ValidateLabel(point.Label);
                }
                break;

            default:
                throw new InvalidOperationException(
                    $"Unsupported mathematical visual kind: {spec.Kind}.");
        }
    }

    private static void ValidateAccessibility(
        MathematicalVisualAccessibility accessibility)
    {
        ArgumentNullException.ThrowIfNull(accessibility);
        if (string.IsNullOrWhiteSpace(accessibility.Title) ||
            string.IsNullOrWhiteSpace(accessibility.Description))
        {
            throw new ArgumentException(
                "Mathematical visuals require a title and description.");
        }

        ValidateLabel(accessibility.Title);
        if (accessibility.Description.Length > 600)
            throw new ArgumentOutOfRangeException(nameof(accessibility.Description));
    }

    private static void ValidateWindow(MathematicalAxisWindow window)
    {
        ArgumentNullException.ThrowIfNull(window);

        ValidateCoordinate(window.XMin, nameof(window.XMin));
        ValidateCoordinate(window.XMax, nameof(window.XMax));
        ValidateCoordinate(window.YMin, nameof(window.YMin));
        ValidateCoordinate(window.YMax, nameof(window.YMax));

        if (window.XMin >= window.XMax ||
            window.YMin >= window.YMax ||
            window.XTick <= 0m ||
            window.YTick <= 0m ||
            window.XMax - window.XMin > MaximumAxisSpan ||
            window.YMax - window.YMin > MaximumAxisSpan)
        {
            throw new ArgumentOutOfRangeException(nameof(window));
        }
    }

    private static void ValidateCurve(MathematicalCurveSpec curve)
    {
        ArgumentNullException.ThrowIfNull(curve);
        if (string.IsNullOrWhiteSpace(curve.Id) ||
            curve.Id.Length > 120)
        {
            throw new ArgumentOutOfRangeException(nameof(curve.Id));
        }

        ValidateLabel(curve.Label);

        switch (curve.Kind)
        {
            case MathematicalCurveKind.Polynomial:
                if (curve.Parameters.Count is < 1 or > 9)
                    throw new ArgumentOutOfRangeException(nameof(curve.Parameters));
                break;

            case MathematicalCurveKind.Sine:
            case MathematicalCurveKind.Cosine:
            case MathematicalCurveKind.Tangent:
                if (curve.Parameters.Count != 4)
                    throw new ArgumentException(
                        "Trigonometric curve parameters must be [amplitude, angularFrequency, phase, verticalShift].");
                break;

            default:
                throw new InvalidOperationException(
                    $"Unsupported mathematical curve kind: {curve.Kind}.");
        }

        foreach (var value in curve.Parameters)
            ValidateCoordinate(value, nameof(curve.Parameters));
    }

    private static void ValidateMatrix(MathematicalMatrix matrix)
    {
        ArgumentNullException.ThrowIfNull(matrix);

        if (matrix.Rows is < 1 or > MaximumMatrixDimension ||
            matrix.Columns is < 1 or > MaximumMatrixDimension ||
            matrix.Values.Count != matrix.Rows * matrix.Columns)
        {
            throw new ArgumentOutOfRangeException(nameof(matrix));
        }

        ValidateLabel(matrix.Label);
        foreach (var value in matrix.Values)
        {
            if (string.IsNullOrWhiteSpace(value) ||
                value.Length > 80)
            {
                throw new ArgumentOutOfRangeException(nameof(matrix.Values));
            }
        }
    }

    private static void ValidatePoints(
        IReadOnlyList<MathematicalPoint> points)
    {
        ArgumentNullException.ThrowIfNull(points);
        if (points.Count > MaximumPoints)
            throw new ArgumentOutOfRangeException(nameof(points));

        foreach (var point in points)
            ValidatePoint(point);
    }

    private static void ValidatePoint(MathematicalPoint point)
    {
        ArgumentNullException.ThrowIfNull(point);
        ValidateCoordinate(point.X, nameof(point.X));
        ValidateCoordinate(point.Y, nameof(point.Y));
        ValidateLabel(point.Label);
    }

    private static void ValidateSegments(
        IReadOnlyList<MathematicalSegment> segments)
    {
        ArgumentNullException.ThrowIfNull(segments);
        if (segments.Count > MaximumSegments)
            throw new ArgumentOutOfRangeException(nameof(segments));

        foreach (var segment in segments)
        {
            ArgumentNullException.ThrowIfNull(segment);
            ValidatePoint(segment.Start);
            ValidatePoint(segment.End);
            ValidateLabel(segment.Label);
        }
    }

    private static void ValidateCoordinate(
        decimal value,
        string parameterName)
    {
        if (value is < -1_000_000m or > 1_000_000m)
            throw new ArgumentOutOfRangeException(parameterName);
    }

    private static void ValidateLabel(string? label)
    {
        if (label is not null && label.Length > MaximumLabelLength)
            throw new ArgumentOutOfRangeException(nameof(label));
    }
}
