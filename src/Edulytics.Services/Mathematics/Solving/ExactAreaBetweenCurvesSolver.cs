using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Solving;

/// <summary>
/// Exact, fail-closed area-between-curves solver for bounded polynomial pairs.
/// The initial certified slice supports polynomial differences through degree 2
/// whose relevant intersections are exact rational numbers.
/// </summary>
public sealed class ExactAreaBetweenCurvesSolver : IMathematicsSolver
{
    private const int MaxDegree = 2;
    private const int MaxBreakpoints = 4;

    public MathematicsSolveResult Solve(
        MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Problem is not AreaBetweenCurvesNode area ||
            area.FirstCurve is null ||
            area.SecondCurve is null ||
            area.Variable is null ||
            area.LowerBound is null ||
            area.UpperBound is null)
        {
            return Unsupported(
                request,
                "Area-between-curves requires two curves, one variable and finite exact bounds.");
        }

        if (!ExactCalculusV2.TryPreflight(
                area.FirstCurve,
                out var preflightError,
                out var resourceLimit) ||
            !ExactCalculusV2.TryPreflight(
                area.SecondCurve,
                out preflightError,
                out resourceLimit) ||
            !ExactCalculusV2.TryPreflight(
                area.LowerBound,
                out preflightError,
                out resourceLimit) ||
            !ExactCalculusV2.TryPreflight(
                area.UpperBound,
                out preflightError,
                out resourceLimit))
        {
            return resourceLimit
                ? ExactCalculusV2.ResourceLimit(
                    request,
                    preflightError)
                : Unsupported(
                    request,
                    preflightError);
        }

        if (!ExactCalculusV2.TryReadScalar(
                area.LowerBound,
                out var lower) ||
            !ExactCalculusV2.TryReadScalar(
                area.UpperBound,
                out var upper) ||
            lower.CompareTo(upper) >= 0)
        {
            return Unsupported(
                request,
                "Area-between-curves bounds must be finite exact scalars with lower < upper.");
        }

        if (!ExactCalculusPolynomial.TryParse(
                area.FirstCurve,
                area.Variable.Name,
                MaxDegree,
                out var first,
                out var firstError,
                out var firstResourceLimit))
        {
            return firstResourceLimit
                ? ExactCalculusV2.ResourceLimit(
                    request,
                    firstError)
                : Unsupported(
                    request,
                    firstError);
        }

        if (!ExactCalculusPolynomial.TryParse(
                area.SecondCurve,
                area.Variable.Name,
                MaxDegree,
                out var second,
                out var secondError,
                out var secondResourceLimit))
        {
            return secondResourceLimit
                ? ExactCalculusV2.ResourceLimit(
                    request,
                    secondError)
                : Unsupported(
                    request,
                    secondError);
        }

        if (!TrySubtractPolynomials(
                first,
                second,
                out var difference,
                out var subtractionError))
        {
            return ExactCalculusV2.ResourceLimit(
                request,
                subtractionError);
        }

        if (!TryExactRationalRoots(
                difference,
                out var roots,
                out var rootError))
        {
            return Unsupported(
                request,
                rootError);
        }

        var breakpoints = new List<ExactRational>
        {
            lower
        };

        foreach (var root in roots
                     .Where(root =>
                         root.CompareTo(lower) > 0 &&
                         root.CompareTo(upper) < 0)
                     .OrderBy(root => root, ExactRationalComparer.Instance))
        {
            if (!breakpoints.Contains(root))
                breakpoints.Add(root);
        }

        breakpoints.Add(upper);

        if (breakpoints.Count > MaxBreakpoints)
        {
            return ExactCalculusV2.ResourceLimit(
                request,
                "Area-between-curves produced more exact sub-intervals than the bounded slice permits.");
        }

        var total = Zero;
        var trace = new List<MathematicsSolutionStep>();

        var rootVector = new VectorNode(
            breakpoints
                .Skip(1)
                .Take(Math.Max(0, breakpoints.Count - 2))
                .Select(ExactCalculusV2.ToNode)
                .ToArray());

        trace.Add(
            new MathematicsSolutionStep(
                "intersections",
                area,
                "solve-f-equals-g",
                "Solve the exact polynomial difference f(x)-g(x)=0 inside the requested interval.",
                rootVector,
                "Only exact rational intersections are accepted by this certified slice.",
                request.Assumptions,
                false));

        for (var i = 0; i < breakpoints.Count - 1; i++)
        {
            var from = breakpoints[i];
            var to = breakpoints[i + 1];

            if (!TryMidpoint(
                    from,
                    to,
                    out var midpoint) ||
                !TryEvaluate(
                    difference,
                    midpoint,
                    out var midpointValue) ||
                !TryIntegrate(
                    difference,
                    from,
                    to,
                    out var signedArea))
            {
                return ExactCalculusV2.ResourceLimit(
                    request,
                    "Area-between-curves exact piecewise evaluation exceeded the arithmetic budget.");
            }

            var pieceArea = midpointValue.Numerator.Sign switch
            {
                < 0 => Negate(signedArea),
                _ => signedArea
            };

            if (pieceArea.Numerator.Sign < 0)
                pieceArea = Negate(pieceArea);

            if (!TryAdd(
                    total,
                    pieceArea,
                    out total))
            {
                return ExactCalculusV2.ResourceLimit(
                    request,
                    "Area-between-curves exact accumulation exceeded the arithmetic budget.");
            }

            trace.Add(
                new MathematicsSolutionStep(
                    $"piece-{i + 1}",
                    new IntegralNode(
                        ExactCalculusV2.ToPolynomialNode(
                            difference,
                            area.Variable.Name),
                        area.Variable,
                        ExactCalculusV2.ToNode(from),
                        ExactCalculusV2.ToNode(to)),
                    "integrate-absolute-difference",
                    midpointValue.Numerator.Sign < 0
                        ? "The second curve is above the first on this root-free interval; integrate g(x)-f(x)."
                        : "The first curve is above the second on this root-free interval; integrate f(x)-g(x).",
                    ExactCalculusV2.ToNode(pieceArea),
                    "The sign is constant between consecutive exact intersections, so the geometric area equals the absolute signed integral.",
                    request.Assumptions,
                    false));
        }

        var resultNode =
            ExactCalculusV2.ToNode(total);

        return new MathematicsSolveResult(
            MathematicsSolveStatus.Solved,
            resultNode,
            new FiniteSolutionSet([resultNode]),
            request.Assumptions,
            "exact-piecewise-absolute-polynomial-area",
            new MathematicsSolutionTrace(trace),
            "edulytics-native-calculus",
            "calculus-area-between-curves-exact-v1",
            []);
    }

    private static bool TrySubtractPolynomials(
        IReadOnlyDictionary<int, ExactRational> first,
        IReadOnlyDictionary<int, ExactRational> second,
        out IReadOnlyDictionary<int, ExactRational> difference,
        out string error)
    {
        var result =
            new Dictionary<int, ExactRational>();

        foreach (var degree in
                 first.Keys.Concat(second.Keys).Distinct())
        {
            var left = first.TryGetValue(
                degree,
                out var firstValue)
                ? firstValue
                : Zero;
            var right = second.TryGetValue(
                degree,
                out var secondValue)
                ? secondValue
                : Zero;

            if (!TrySubtract(
                    left,
                    right,
                    out var value))
            {
                difference =
                    new Dictionary<int, ExactRational>();
                error =
                    "Polynomial subtraction exceeded the exact arithmetic budget.";
                return false;
            }

            if (value != Zero)
                result[degree] = value;
        }

        difference = result;
        error = string.Empty;
        return true;
    }

    private static bool TryExactRationalRoots(
        IReadOnlyDictionary<int, ExactRational> polynomial,
        out IReadOnlyList<ExactRational> roots,
        out string error)
    {
        var degree = polynomial.Count == 0
            ? 0
            : polynomial.Keys.Max();

        if (degree == 0)
        {
            if (polynomial.Count == 0)
            {
                roots = [];
                error =
                    "The two curves are identical on the interval; this family requires distinct polynomial curves.";
                return false;
            }

            roots = [];
            error = string.Empty;
            return true;
        }

        var a = Get(polynomial, 2);
        var b = Get(polynomial, 1);
        var c = Get(polynomial, 0);

        if (degree == 1)
        {
            if (b == Zero ||
                !TryDivide(
                    Negate(c),
                    b,
                    out var root))
            {
                roots = [];
                error =
                    "The exact linear intersection could not be represented within the arithmetic budget.";
                return false;
            }

            roots = [root];
            error = string.Empty;
            return true;
        }

        if (degree != 2 || a == Zero)
        {
            roots = [];
            error =
                "Area-between-curves currently certifies polynomial differences through degree 2 only.";
            return false;
        }

        if (!TryMultiply(
                b,
                b,
                out var bSquared) ||
            !TryMultiply(
                new ExactRational(4, 1),
                a,
                out var fourA) ||
            !TryMultiply(
                fourA,
                c,
                out var fourAC) ||
            !TrySubtract(
                bSquared,
                fourAC,
                out var discriminant))
        {
            roots = [];
            error =
                "Quadratic intersection discriminant exceeded the exact arithmetic budget.";
            return false;
        }

        if (discriminant.Numerator.Sign < 0)
        {
            roots = [];
            error = string.Empty;
            return true;
        }

        if (!TryExactSquareRoot(
                discriminant,
                out var squareRoot))
        {
            roots = [];
            error =
                "Quadratic intersections are not exact rationals in the current certified slice.";
            return false;
        }

        if (!TryMultiply(
                new ExactRational(2, 1),
                a,
                out var denominator) ||
            denominator == Zero)
        {
            roots = [];
            error =
                "Quadratic intersection denominator is invalid.";
            return false;
        }

        var negativeB = Negate(b);

        if (!TryAdd(
                negativeB,
                squareRoot,
                out var plusNumerator) ||
            !TrySubtract(
                negativeB,
                squareRoot,
                out var minusNumerator) ||
            !TryDivide(
                plusNumerator,
                denominator,
                out var firstRoot) ||
            !TryDivide(
                minusNumerator,
                denominator,
                out var secondRoot))
        {
            roots = [];
            error =
                "Quadratic intersection roots exceeded the exact arithmetic budget.";
            return false;
        }

        roots =
            firstRoot == secondRoot
                ? [firstRoot]
                : new[] { firstRoot, secondRoot }
                    .OrderBy(
                        root => root,
                        ExactRationalComparer.Instance)
                    .ToArray();

        error = string.Empty;
        return true;
    }

    private static bool TryIntegrate(
        IReadOnlyDictionary<int, ExactRational> polynomial,
        ExactRational lower,
        ExactRational upper,
        out ExactRational result)
    {
        result = Zero;

        foreach (var pair in polynomial)
        {
            var degree = pair.Key + 1;

            if (!TryDivide(
                    pair.Value,
                    new ExactRational(degree, 1),
                    out var antiCoefficient) ||
                !TryPow(
                    upper,
                    degree,
                    out var upperPower) ||
                !TryPow(
                    lower,
                    degree,
                    out var lowerPower) ||
                !TrySubtract(
                    upperPower,
                    lowerPower,
                    out var delta) ||
                !TryMultiply(
                    antiCoefficient,
                    delta,
                    out var term) ||
                !TryAdd(
                    result,
                    term,
                    out result))
            {
                return false;
            }
        }

        return true;
    }

    private static bool TryEvaluate(
        IReadOnlyDictionary<int, ExactRational> polynomial,
        ExactRational x,
        out ExactRational result)
    {
        result = Zero;
        var degree =
            polynomial.Count == 0
                ? 0
                : polynomial.Keys.Max();

        for (var power = degree; power >= 0; power--)
        {
            if (!TryMultiply(
                    result,
                    x,
                    out result) ||
                !TryAdd(
                    result,
                    Get(polynomial, power),
                    out result))
            {
                return false;
            }
        }

        return true;
    }

    private static bool TryMidpoint(
        ExactRational left,
        ExactRational right,
        out ExactRational midpoint)
    {
        midpoint = default;
        return TryAdd(
                   left,
                   right,
                   out var total) &&
               TryDivide(
                   total,
                   new ExactRational(2, 1),
                   out midpoint);
    }

    private static bool TryExactSquareRoot(
        ExactRational value,
        out ExactRational root)
    {
        root = default;

        if (value.Numerator.Sign < 0 ||
            !TryIntegerSquareRoot(
                BigInteger.Abs(value.Numerator),
                out var numeratorRoot) ||
            !TryIntegerSquareRoot(
                value.Denominator,
                out var denominatorRoot))
        {
            return false;
        }

        root = new ExactRational(
            numeratorRoot,
            denominatorRoot);
        return IsBounded(root);
    }

    private static bool TryIntegerSquareRoot(
        BigInteger value,
        out BigInteger root)
    {
        root = BigInteger.Zero;

        if (value.Sign < 0)
            return false;

        if (value <= BigInteger.One)
        {
            root = value;
            return true;
        }

        var low = BigInteger.Zero;
        var high = BigInteger.One;

        while (high * high < value)
        {
            high <<= 1;
            if (BitLength(high) > 4096)
                return false;
        }

        while (high - low > BigInteger.One)
        {
            var middle = (low + high) >> 1;
            var square = middle * middle;

            if (square == value)
            {
                root = middle;
                return true;
            }

            if (square < value)
                low = middle;
            else
                high = middle;
        }

        if (low * low == value)
        {
            root = low;
            return true;
        }

        if (high * high == value)
        {
            root = high;
            return true;
        }

        return false;
    }

    private static ExactRational Get(
        IReadOnlyDictionary<int, ExactRational> polynomial,
        int degree) =>
        polynomial.TryGetValue(
            degree,
            out var value)
            ? value
            : Zero;

    private static ExactRational Negate(
        ExactRational value) =>
        new(
            BigInteger.Negate(value.Numerator),
            value.Denominator);

    private static bool TryAdd(
        ExactRational left,
        ExactRational right,
        out ExactRational result) =>
        BoundedArithmetic.TryAdd(
            left,
            right,
            out result);

    private static bool TrySubtract(
        ExactRational left,
        ExactRational right,
        out ExactRational result) =>
        BoundedArithmetic.TryAdd(
            left,
            Negate(right),
            out result);

    private static bool TryMultiply(
        ExactRational left,
        ExactRational right,
        out ExactRational result) =>
        BoundedArithmetic.TryMultiply(
            left,
            right,
            out result);

    private static bool TryDivide(
        ExactRational left,
        ExactRational right,
        out ExactRational result) =>
        BoundedArithmetic.TryDivide(
            left,
            right,
            out result);

    private static bool TryPow(
        ExactRational value,
        int exponent,
        out ExactRational result) =>
        BoundedArithmetic.TryPow(
            value,
            exponent,
            out result);

    private static bool IsBounded(
        ExactRational value) =>
        BoundedArithmetic.IsValid(value);

    private static MathematicsSolveResult Unsupported(
        MathematicsSolveRequest request,
        string diagnostic) =>
        new(
            MathematicsSolveStatus.Unsupported,
            null,
            null,
            request.Assumptions,
            null,
            null,
            "edulytics-native-calculus",
            "calculus-area-between-curves-exact-v1",
            [diagnostic]);

    private static long BitLength(
        BigInteger value) =>
        value.IsZero
            ? 0
            : BigInteger.Abs(value).GetBitLength();

    private sealed class ExactRationalComparer
        : IComparer<ExactRational>
    {
        public static ExactRationalComparer Instance { get; } =
            new();

        public int Compare(
            ExactRational x,
            ExactRational y) =>
            x.CompareTo(y);
    }

    private static class BoundedArithmetic
    {
        private const int MaxBits = 4096;
        private const long MaxIntermediateBits =
            (MaxBits * 2L) + 1L;

        public static bool IsValid(
            ExactRational value) =>
            !value.Denominator.IsZero &&
            BitLength(value.Numerator) <= MaxBits &&
            BitLength(value.Denominator) <= MaxBits;

        public static bool TryAdd(
            ExactRational left,
            ExactRational right,
            out ExactRational result)
        {
            result = default;

            if (!IsValid(left) ||
                !IsValid(right))
                return false;

            var numeratorBits =
                Math.Max(
                    SafeAdd(
                        BitLength(left.Numerator),
                        BitLength(right.Denominator)),
                    SafeAdd(
                        BitLength(right.Numerator),
                        BitLength(left.Denominator))) + 1;
            var denominatorBits =
                SafeAdd(
                    BitLength(left.Denominator),
                    BitLength(right.Denominator));

            if (numeratorBits > MaxIntermediateBits ||
                denominatorBits > MaxIntermediateBits)
                return false;

            result = left + right;
            return IsValid(result);
        }

        public static bool TryMultiply(
            ExactRational left,
            ExactRational right,
            out ExactRational result)
        {
            result = default;

            if (!IsValid(left) ||
                !IsValid(right) ||
                SafeAdd(
                    BitLength(left.Numerator),
                    BitLength(right.Numerator)) >
                MaxIntermediateBits ||
                SafeAdd(
                    BitLength(left.Denominator),
                    BitLength(right.Denominator)) >
                MaxIntermediateBits)
                return false;

            result = left * right;
            return IsValid(result);
        }

        public static bool TryDivide(
            ExactRational left,
            ExactRational right,
            out ExactRational result)
        {
            result = default;

            if (!IsValid(left) ||
                !IsValid(right) ||
                right.Numerator.IsZero ||
                SafeAdd(
                    BitLength(left.Numerator),
                    BitLength(right.Denominator)) >
                MaxIntermediateBits ||
                SafeAdd(
                    BitLength(left.Denominator),
                    BitLength(right.Numerator)) >
                MaxIntermediateBits)
                return false;

            result = left / right;
            return IsValid(result);
        }

        public static bool TryPow(
            ExactRational value,
            int exponent,
            out ExactRational result)
        {
            result =
                new ExactRational(
                    BigInteger.One,
                    BigInteger.One);

            if (!IsValid(value) ||
                exponent is < 0 or > 4)
                return false;

            for (var i = 0; i < exponent; i++)
            {
                if (!TryMultiply(
                        result,
                        value,
                        out result))
                    return false;
            }

            return true;
        }

        private static long SafeAdd(
            long left,
            long right) =>
            left > long.MaxValue - right
                ? long.MaxValue
                : left + right;
    }

    private static readonly ExactRational Zero =
        new(
            BigInteger.Zero,
            BigInteger.One);
}
