using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Core.Mathematics.Verification;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Verification;

/// <summary>
/// Independent verification for the bounded exact area-between-curves slice.
/// It deliberately uses the verifier polynomial parser/arithmetic rather than
/// the solver implementation.
/// </summary>
public sealed class ExactAreaBetweenCurvesVerifier
    : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(
        MathematicsSolveRequest request,
        MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(result);

        if (request.Problem is not AreaBetweenCurvesNode area ||
            area.FirstCurve is null ||
            area.SecondCurve is null ||
            area.Variable is null ||
            area.LowerBound is null ||
            area.UpperBound is null)
        {
            return Unsupported(
                "Independent area verification requires a well-formed AreaBetweenCurvesNode.");
        }

        if (!VerifierPolynomial.TryParse(
                area.FirstCurve,
                area.Variable.Name,
                2,
                out var first,
                out var firstError))
        {
            return Unsupported(firstError);
        }

        if (!VerifierPolynomial.TryParse(
                area.SecondCurve,
                area.Variable.Name,
                2,
                out var second,
                out var secondError))
        {
            return Unsupported(secondError);
        }

        if (!VerifierCalculusV2.TryReadScalar(
                area.LowerBound,
                out var lower) ||
            !VerifierCalculusV2.TryReadScalar(
                area.UpperBound,
                out var upper) ||
            lower.CompareTo(upper) >= 0)
        {
            return Unsupported(
                "Independent area verification requires finite exact bounds with lower < upper.");
        }

        if (!TryDifference(
                first,
                second,
                out var difference))
        {
            return Unsupported(
                "Independent area verification exceeded its exact subtraction budget.");
        }

        if (!TryRoots(
                difference,
                out var roots,
                out var rootDiagnostic))
        {
            return Unsupported(rootDiagnostic);
        }

        var breakpoints =
            new List<ExactRational>
            {
                lower
            };

        foreach (var root in roots
                     .Where(root =>
                         root.CompareTo(lower) > 0 &&
                         root.CompareTo(upper) < 0)
                     .OrderBy(
                         root => root,
                         RationalComparer.Instance))
        {
            if (!breakpoints.Contains(root))
                breakpoints.Add(root);
        }

        breakpoints.Add(upper);

        if (breakpoints.Count > 4)
        {
            return Unsupported(
                "Independent area verification exceeded its bounded number of sub-intervals.");
        }

        var expected = Zero;

        for (var i = 0; i < breakpoints.Count - 1; i++)
        {
            var from = breakpoints[i];
            var to = breakpoints[i + 1];

            if (!TryAverage(
                    from,
                    to,
                    out var probe) ||
                !TryEvaluate(
                    difference,
                    probe,
                    out var signValue) ||
                !TryIntegral(
                    difference,
                    from,
                    to,
                    out var signedPiece))
            {
                return Unsupported(
                    "Independent area verification exceeded its exact arithmetic budget.");
            }

            var piece = signValue.Numerator.Sign < 0
                ? Negate(signedPiece)
                : signedPiece;

            if (piece.Numerator.Sign < 0)
                piece = Negate(piece);

            if (!VerifierCalcArithmetic.TryAdd(
                    expected,
                    piece,
                    out expected))
            {
                return Unsupported(
                    "Independent area verification could not accumulate the exact area.");
            }
        }

        if (!VerifierCalculusV2.TryReadSolvedScalar(
                result,
                out var reported,
                out var resultError))
        {
            return Rejected(resultError);
        }

        if (reported != expected)
        {
            return Rejected(
                "Reported area does not equal the independently recomputed piecewise integral of |f(x)-g(x)|.");
        }

        if (result.Trace is null ||
            result.Trace.Steps.Count < 2 ||
            !result.Trace.Steps.Any(step =>
                string.Equals(
                    step.StepId,
                    "intersections",
                    StringComparison.Ordinal)))
        {
            return Rejected(
                "Area-between-curves result is missing the required structured intersection/piecewise solution trace.");
        }

        return new MathematicsVerificationResult(
            MathematicsVerificationStatus.Verified,
            [
                new MathematicsVerificationEvidence(
                    "independent-piecewise-absolute-polynomial-integration",
                    "The verifier independently parsed both curves, solved exact rational intersections, split the interval, determined ordering on each root-free interval and recomputed the geometric area.")
            ],
            []);
    }

    private static bool TryDifference(
        IReadOnlyDictionary<int, ExactRational> first,
        IReadOnlyDictionary<int, ExactRational> second,
        out IReadOnlyDictionary<int, ExactRational> difference)
    {
        var result =
            new Dictionary<int, ExactRational>();

        foreach (var degree in
                 first.Keys.Concat(second.Keys).Distinct())
        {
            var left = Get(first, degree);
            var right = Get(second, degree);

            if (!VerifierCalcArithmetic.TrySubtract(
                    left,
                    right,
                    out var value))
            {
                difference =
                    new Dictionary<int, ExactRational>();
                return false;
            }

            if (value != Zero)
                result[degree] = value;
        }

        difference = result;
        return true;
    }

    private static bool TryRoots(
        IReadOnlyDictionary<int, ExactRational> polynomial,
        out IReadOnlyList<ExactRational> roots,
        out string diagnostic)
    {
        var degree =
            polynomial.Count == 0
                ? 0
                : polynomial.Keys.Max();

        if (degree == 0)
        {
            if (polynomial.Count == 0)
            {
                roots = [];
                diagnostic =
                    "Independent area verifier rejects identical curves in this certified family.";
                return false;
            }

            roots = [];
            diagnostic = string.Empty;
            return true;
        }

        var a = Get(polynomial, 2);
        var b = Get(polynomial, 1);
        var c = Get(polynomial, 0);

        if (degree == 1)
        {
            if (b == Zero ||
                !VerifierCalcArithmetic.TryDivide(
                    Negate(c),
                    b,
                    out var root))
            {
                roots = [];
                diagnostic =
                    "Independent linear intersection could not be represented exactly.";
                return false;
            }

            roots = [root];
            diagnostic = string.Empty;
            return true;
        }

        if (degree != 2 || a == Zero)
        {
            roots = [];
            diagnostic =
                "Independent area verifier supports polynomial differences through degree 2 only.";
            return false;
        }

        if (!VerifierCalcArithmetic.TryMultiply(
                b,
                b,
                out var bSquared) ||
            !VerifierCalcArithmetic.TryMultiply(
                new ExactRational(4, 1),
                a,
                out var fourA) ||
            !VerifierCalcArithmetic.TryMultiply(
                fourA,
                c,
                out var fourAC) ||
            !VerifierCalcArithmetic.TrySubtract(
                bSquared,
                fourAC,
                out var discriminant))
        {
            roots = [];
            diagnostic =
                "Independent quadratic intersection discriminant exceeded the arithmetic budget.";
            return false;
        }

        if (discriminant.Numerator.Sign < 0)
        {
            roots = [];
            diagnostic = string.Empty;
            return true;
        }

        if (!TrySquareRoot(
                discriminant,
                out var squareRoot))
        {
            roots = [];
            diagnostic =
                "Independent area verifier requires exact rational intersections.";
            return false;
        }

        if (!VerifierCalcArithmetic.TryMultiply(
                new ExactRational(2, 1),
                a,
                out var denominator) ||
            denominator == Zero)
        {
            roots = [];
            diagnostic =
                "Independent quadratic intersection denominator is invalid.";
            return false;
        }

        var negativeB = Negate(b);

        if (!VerifierCalcArithmetic.TryAdd(
                negativeB,
                squareRoot,
                out var plus) ||
            !VerifierCalcArithmetic.TrySubtract(
                negativeB,
                squareRoot,
                out var minus) ||
            !VerifierCalcArithmetic.TryDivide(
                plus,
                denominator,
                out var first) ||
            !VerifierCalcArithmetic.TryDivide(
                minus,
                denominator,
                out var second))
        {
            roots = [];
            diagnostic =
                "Independent quadratic roots exceeded the arithmetic budget.";
            return false;
        }

        roots =
            first == second
                ? [first]
                : new[] { first, second }
                    .OrderBy(
                        value => value,
                        RationalComparer.Instance)
                    .ToArray();

        diagnostic = string.Empty;
        return true;
    }

    private static bool TryIntegral(
        IReadOnlyDictionary<int, ExactRational> polynomial,
        ExactRational lower,
        ExactRational upper,
        out ExactRational result)
    {
        result = Zero;

        foreach (var pair in polynomial)
        {
            var degree = pair.Key + 1;

            if (!VerifierCalcArithmetic.TryDivide(
                    pair.Value,
                    new ExactRational(degree, 1),
                    out var antiCoefficient) ||
                !VerifierCalcArithmetic.TryPow(
                    upper,
                    degree,
                    out var upperPower) ||
                !VerifierCalcArithmetic.TryPow(
                    lower,
                    degree,
                    out var lowerPower) ||
                !VerifierCalcArithmetic.TrySubtract(
                    upperPower,
                    lowerPower,
                    out var powerDifference) ||
                !VerifierCalcArithmetic.TryMultiply(
                    antiCoefficient,
                    powerDifference,
                    out var term) ||
                !VerifierCalcArithmetic.TryAdd(
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
            if (!VerifierCalcArithmetic.TryMultiply(
                    result,
                    x,
                    out result) ||
                !VerifierCalcArithmetic.TryAdd(
                    result,
                    Get(polynomial, power),
                    out result))
            {
                return false;
            }
        }

        return true;
    }

    private static bool TryAverage(
        ExactRational left,
        ExactRational right,
        out ExactRational average)
    {
        average = default;

        return VerifierCalcArithmetic.TryAdd(
                   left,
                   right,
                   out var total) &&
               VerifierCalcArithmetic.TryDivide(
                   total,
                   new ExactRational(2, 1),
                   out average);
    }

    private static bool TrySquareRoot(
        ExactRational value,
        out ExactRational root)
    {
        root = default;

        if (value.Numerator.Sign < 0 ||
            !TryIntegerSqrt(
                BigInteger.Abs(value.Numerator),
                out var numerator) ||
            !TryIntegerSqrt(
                value.Denominator,
                out var denominator))
        {
            return false;
        }

        root = new ExactRational(
            numerator,
            denominator);
        return VerifierCalcArithmetic.IsValid(root);
    }

    private static bool TryIntegerSqrt(
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

        var estimate =
            BigInteger.One <<
            (int)((value.GetBitLength() + 1) / 2);

        for (var i = 0; i < 32; i++)
        {
            var next =
                (estimate + value / estimate) >> 1;

            if (next == estimate ||
                next == estimate - BigInteger.One)
                break;

            estimate = next;
        }

        while (estimate * estimate > value)
            estimate -= BigInteger.One;

        while ((estimate + BigInteger.One) *
               (estimate + BigInteger.One) <= value)
            estimate += BigInteger.One;

        if (estimate * estimate != value)
            return false;

        root = estimate;
        return true;
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

    private static MathematicsVerificationResult Rejected(
        string diagnostic) =>
        new(
            MathematicsVerificationStatus.Rejected,
            [],
            [diagnostic]);

    private static MathematicsVerificationResult Unsupported(
        string diagnostic) =>
        new(
            MathematicsVerificationStatus.Unsupported,
            [],
            [diagnostic]);

    private sealed class RationalComparer
        : IComparer<ExactRational>
    {
        public static RationalComparer Instance { get; } =
            new();

        public int Compare(
            ExactRational x,
            ExactRational y) =>
            x.CompareTo(y);
    }

    private static readonly ExactRational Zero =
        new(
            BigInteger.Zero,
            BigInteger.One);
}
