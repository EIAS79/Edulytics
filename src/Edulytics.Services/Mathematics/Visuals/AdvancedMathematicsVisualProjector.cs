using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Core.Mathematics.Visuals;
using Edulytics.Services.Mathematics.Solving;

namespace Edulytics.Services.Mathematics.Visuals;

/// <summary>
/// Presentation-only projection from exact advanced mathematics problems into
/// typed visual contracts. The projection never acts as the mathematical
/// authority and fails closed when exact values cannot be represented safely
/// for deterministic display.
/// </summary>
public static class AdvancedMathematicsVisualProjector
{
    public static bool TryProjectAreaBetweenCurves(
        AreaBetweenCurvesNode problem,
        MathematicsSolveResult solution,
        out RegionOfIntegrationVisualSpec? visual)
    {
        ArgumentNullException.ThrowIfNull(problem);
        ArgumentNullException.ThrowIfNull(solution);

        visual = null;

        var normalizer =
            new ExactPolynomialNormalizer();

        var first =
            normalizer.Normalize(
                problem.FirstCurve,
                problem.Variable.Name,
                2);
        var second =
            normalizer.Normalize(
                problem.SecondCurve,
                problem.Variable.Name,
                2);

        if (!first.IsSupported ||
            !second.IsSupported ||
            !TryDecimal(
                problem.LowerBound,
                out var lower) ||
            !TryDecimal(
                problem.UpperBound,
                out var upper) ||
            lower >= upper)
        {
            return false;
        }

        if (!TryCoefficients(
                first.Coefficients,
                out var firstCoefficients) ||
            !TryCoefficients(
                second.Coefficients,
                out var secondCoefficients))
        {
            return false;
        }

        var intersections =
            ReadIntersections(
                solution,
                firstCoefficients);

        var values =
            new List<decimal>();

        const int samples = 64;
        for (var index = 0; index <= samples; index++)
        {
            var x =
                lower +
                (upper - lower) *
                index /
                samples;

            if (!TryEvaluate(
                    firstCoefficients,
                    x,
                    out var firstY) ||
                !TryEvaluate(
                    secondCoefficients,
                    x,
                    out var secondY))
            {
                return false;
            }

            values.Add(firstY);
            values.Add(secondY);
        }

        if (values.Count == 0)
            return false;

        var yMin = values.Min();
        var yMax = values.Max();

        if (yMin == yMax)
        {
            yMin -= 1m;
            yMax += 1m;
        }

        var yPadding =
            Math.Max(
                1m,
                (yMax - yMin) * 0.12m);

        var xSpan = upper - lower;
        var ySpan =
            (yMax + yPadding) -
            (yMin - yPadding);

        visual =
            new RegionOfIntegrationVisualSpec(
                new MathematicalAxisWindow(
                    lower,
                    upper,
                    yMin - yPadding,
                    yMax + yPadding,
                    Tick(xSpan),
                    Tick(ySpan)),
                new MathematicalCurveSpec(
                    "curve-f",
                    MathematicalCurveKind.Polynomial,
                    firstCoefficients,
                    "f(x)"),
                new MathematicalCurveSpec(
                    "curve-g",
                    MathematicalCurveKind.Polynomial,
                    secondCoefficients,
                    "g(x)"),
                lower,
                upper,
                intersections,
                new MathematicalVisualAccessibility(
                    "Area between two curves",
                    "Two deterministic polynomial curves are shown over the requested interval. The enclosed geometric region is shaded separately between exact intersection points."));

        return true;
    }

    private static IReadOnlyList<MathematicalPoint>
        ReadIntersections(
            MathematicsSolveResult solution,
            IReadOnlyList<decimal> firstCoefficients)
    {
        var step =
            solution.Trace?.Steps
                .FirstOrDefault(row =>
                    string.Equals(
                        row.StepId,
                        "intersections",
                        StringComparison.Ordinal));

        if (step?.After is not VectorNode vector)
            return [];

        var result =
            new List<MathematicalPoint>();

        foreach (var component in vector.Components)
        {
            if (!TryDecimal(
                    component,
                    out var x) ||
                !TryEvaluate(
                    firstCoefficients,
                    x,
                    out var y))
            {
                continue;
            }

            result.Add(
                new MathematicalPoint(
                    x,
                    y,
                    $"({Format(x)}, {Format(y)})"));
        }

        return result;
    }

    private static bool TryCoefficients(
        IReadOnlyDictionary<int, ExactRational> source,
        out IReadOnlyList<decimal> coefficients)
    {
        var result =
            new decimal[3];

        for (var degree = 0; degree <= 2; degree++)
        {
            var value =
                source.TryGetValue(
                    degree,
                    out var found)
                    ? found
                    : new ExactRational(
                        BigInteger.Zero,
                        BigInteger.One);

            if (!TryDecimal(
                    value,
                    out result[degree]))
            {
                coefficients = [];
                return false;
            }
        }

        coefficients = result;
        return true;
    }

    private static bool TryEvaluate(
        IReadOnlyList<decimal> coefficients,
        decimal x,
        out decimal value)
    {
        value = 0m;

        try
        {
            for (var index =
                     coefficients.Count - 1;
                 index >= 0;
                 index--)
            {
                checked
                {
                    value =
                        value * x +
                        coefficients[index];
                }
            }

            return decimal.Abs(value) <=
                   1_000_000m;
        }
        catch (OverflowException)
        {
            value = 0m;
            return false;
        }
    }

    private static bool TryDecimal(
        MathNode node,
        out decimal value)
    {
        switch (node)
        {
            case IntegerNode integer:
                try
                {
                    value =
                        (decimal)integer.Value;
                    return decimal.Abs(value) <=
                           1_000_000m;
                }
                catch (OverflowException)
                {
                    value = 0m;
                    return false;
                }

            case RationalNode rational:
                return TryDecimal(
                    rational.Value,
                    out value);

            default:
                value = 0m;
                return false;
        }
    }

    private static bool TryDecimal(
        ExactRational rational,
        out decimal value)
    {
        try
        {
            if (rational.Denominator.IsZero)
            {
                value = 0m;
                return false;
            }

            value =
                (decimal)rational.Numerator /
                (decimal)rational.Denominator;

            return decimal.Abs(value) <=
                   1_000_000m;
        }
        catch (OverflowException)
        {
            value = 0m;
            return false;
        }
    }

    private static decimal Tick(decimal span)
    {
        if (span <= 8m)
            return 1m;
        if (span <= 20m)
            return 2m;
        if (span <= 50m)
            return 5m;
        if (span <= 100m)
            return 10m;

        return Math.Max(
            1m,
            Math.Ceiling(span / 12m));
    }

    private static string Format(decimal value) =>
        value.ToString(
            "0.###",
            System.Globalization.CultureInfo.InvariantCulture);
}
