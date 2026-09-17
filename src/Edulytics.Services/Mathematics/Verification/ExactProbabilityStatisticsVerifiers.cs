using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Core.Mathematics.Verification;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Verification;

public sealed class ExactSimpleProbabilityVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request); ArgumentNullException.ThrowIfNull(result);
        if (request.Problem is not FunctionCallNode call || call.FunctionName != "probability_favourable_over_total_exact" || call.Arguments.Count != 2)
            return ProbabilityStatisticsVerification.Unsupported("Independent simple-probability verification requires the original favourable/total request.");
        if (!ProbabilityStatisticsVerification.TryReadInteger(call.Arguments[0], out var favourable)
            || !ProbabilityStatisticsVerification.TryReadInteger(call.Arguments[1], out var total)
            || total.Sign <= 0 || favourable.Sign < 0 || favourable > total)
            return ProbabilityStatisticsVerification.Unsupported("Original simple-probability counts are outside the supported exact domain.");
        var expected = new ExactRational(favourable, total);
        return ProbabilityStatisticsVerification.CompareSolved(result, expected, "independent-favourable-over-total-recomputation");
    }
}

public sealed class ExactProbabilityComplementVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request); ArgumentNullException.ThrowIfNull(result);
        if (request.Problem is not FunctionCallNode call || call.FunctionName != "probability_complement_exact" || call.Arguments.Count != 1
            || !ProbabilityStatisticsVerification.TryReadScalar(call.Arguments[0], out var probability)
            || probability.Numerator.Sign < 0 || probability.Numerator > probability.Denominator)
            return ProbabilityStatisticsVerification.Unsupported("Independent complement verification requires an original exact probability in [0,1].");
        if (!ProbabilityStatisticsVerificationArithmetic.TrySubtract(ProbabilityStatisticsVerification.One, probability, out var expected))
            return ProbabilityStatisticsVerification.Unsupported("Independent complement recomputation exceeded its exact arithmetic budget.");
        return ProbabilityStatisticsVerification.CompareSolved(result, expected, "independent-probability-complement-recomputation");
    }
}

public sealed class ExactArithmeticMeanVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request); ArgumentNullException.ThrowIfNull(result);
        if (request.Problem is not FunctionCallNode call || call.FunctionName != "statistics_mean_exact" || call.Arguments.Count != 1
            || !ProbabilityStatisticsVerification.TryReadDataVector(call.Arguments[0], out var values))
            return ProbabilityStatisticsVerification.Unsupported("Independent mean verification requires the original bounded exact data vector.");
        var sum = ProbabilityStatisticsVerification.Zero;
        foreach (var value in values)
            if (!ProbabilityStatisticsVerificationArithmetic.TryAdd(sum, value, out sum))
                return ProbabilityStatisticsVerification.Unsupported("Independent mean recomputation exceeded its exact arithmetic budget.");
        if (!ProbabilityStatisticsVerificationArithmetic.TryDivide(sum, new ExactRational(values.Length, BigInteger.One), out var expected))
            return ProbabilityStatisticsVerification.Unsupported("Independent mean division exceeded its exact arithmetic budget.");
        return ProbabilityStatisticsVerification.CompareSolved(result, expected, "independent-arithmetic-mean-recomputation");
    }
}

public sealed class ExactFrequencyMeanVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request); ArgumentNullException.ThrowIfNull(result);
        if (request.Problem is not FunctionCallNode call || call.FunctionName != "statistics_frequency_mean_exact" || call.Arguments.Count != 1
            || !ProbabilityStatisticsVerification.TryReadFrequencyTable(call.Arguments[0], out var values, out var frequencies))
            return ProbabilityStatisticsVerification.Unsupported("Independent frequency-mean verification requires the original bounded exact frequency table.");
        var weightedSum = ProbabilityStatisticsVerification.Zero;
        var totalFrequency = BigInteger.Zero;
        for (var i = 0; i < values.Length; i++)
        {
            if (!ProbabilityStatisticsVerificationArithmetic.TryMultiply(values[i], new ExactRational(frequencies[i], BigInteger.One), out var term)
                || !ProbabilityStatisticsVerificationArithmetic.TryAdd(weightedSum, term, out weightedSum))
                return ProbabilityStatisticsVerification.Unsupported("Independent frequency-mean recomputation exceeded its exact arithmetic budget.");
            totalFrequency += frequencies[i];
        }
        if (totalFrequency.Sign <= 0)
            return ProbabilityStatisticsVerification.Unsupported("Independent frequency-mean verification requires positive total frequency.");
        if (!ProbabilityStatisticsVerificationArithmetic.TryDivide(weightedSum, new ExactRational(totalFrequency, BigInteger.One), out var expected))
            return ProbabilityStatisticsVerification.Unsupported("Independent frequency-mean division exceeded its exact arithmetic budget.");
        return ProbabilityStatisticsVerification.CompareSolved(result, expected, "independent-frequency-table-mean-recomputation");
    }
}

internal static class ProbabilityStatisticsVerificationArithmetic
{
    private const int MaxBits = 4096;
    private const long MaxIntermediateBits = (MaxBits * 2L) + 1L;
    public static bool TryAdd(ExactRational left, ExactRational right, out ExactRational result) => TryAddSubtract(left, right, false, out result);
    public static bool TrySubtract(ExactRational left, ExactRational right, out ExactRational result) => TryAddSubtract(left, right, true, out result);
    private static bool TryAddSubtract(ExactRational left, ExactRational right, bool subtract, out ExactRational result)
    {
        result = default;
        if (!IsValid(left) || !IsValid(right)) return false;
        var lb = AddBits(Bits(left.Numerator), Bits(right.Denominator));
        var rb = AddBits(Bits(right.Numerator), Bits(left.Denominator));
        if (AddBits(Math.Max(lb, rb), 1) > MaxIntermediateBits || AddBits(Bits(left.Denominator), Bits(right.Denominator)) > MaxIntermediateBits) return false;
        result = subtract ? left - right : left + right;
        return IsValid(result);
    }
    public static bool TryMultiply(ExactRational left, ExactRational right, out ExactRational result)
    {
        result = default;
        if (!IsValid(left) || !IsValid(right)
            || AddBits(Bits(left.Numerator), Bits(right.Numerator)) > MaxIntermediateBits
            || AddBits(Bits(left.Denominator), Bits(right.Denominator)) > MaxIntermediateBits) return false;
        result = left * right;
        return IsValid(result);
    }
    public static bool TryDivide(ExactRational left, ExactRational right, out ExactRational result)
    {
        result = default;
        if (!IsValid(left) || !IsValid(right) || right.Numerator.IsZero
            || AddBits(Bits(left.Numerator), Bits(right.Denominator)) > MaxIntermediateBits
            || AddBits(Bits(left.Denominator), Bits(right.Numerator)) > MaxIntermediateBits) return false;
        result = left / right;
        return IsValid(result);
    }
    public static bool IsValid(ExactRational value) => !value.Denominator.IsZero && Bits(value.Numerator) <= MaxBits && Bits(value.Denominator) <= MaxBits;
    private static long Bits(BigInteger value) => value.IsZero ? 0 : BigInteger.Abs(value).GetBitLength();
    private static long AddBits(long a, long b) => a > long.MaxValue - b ? long.MaxValue : a + b;
}

internal static class ProbabilityStatisticsVerification
{
    public static readonly ExactRational Zero = new(BigInteger.Zero, BigInteger.One);
    public static readonly ExactRational One = new(BigInteger.One, BigInteger.One);

    public static bool TryReadScalar(MathNode node, out ExactRational value)
    {
        switch (node)
        {
            case IntegerNode integer: value = new ExactRational(integer.Value, BigInteger.One); return ProbabilityStatisticsVerificationArithmetic.IsValid(value);
            case RationalNode rational when !rational.Value.Denominator.IsZero: value = rational.Value; return ProbabilityStatisticsVerificationArithmetic.IsValid(value);
            default: value = default; return false;
        }
    }
    public static bool TryReadInteger(MathNode node, out BigInteger value)
    {
        value = default;
        if (node is not IntegerNode integer) return false;
        var exact = new ExactRational(integer.Value, BigInteger.One);
        if (!ProbabilityStatisticsVerificationArithmetic.IsValid(exact)) return false;
        value = integer.Value;
        return true;
    }
    public static bool TryReadDataVector(MathNode node, out ExactRational[] values)
    {
        values = [];
        if (node is not VectorNode vector || vector.Components.Count is < 1 or > 8) return false;
        values = new ExactRational[vector.Components.Count];
        for (var i = 0; i < values.Length; i++) if (!TryReadScalar(vector.Components[i], out values[i])) return false;
        return true;
    }
    public static bool TryReadFrequencyTable(MathNode node, out ExactRational[] values, out BigInteger[] frequencies)
    {
        values = []; frequencies = [];
        if (node is not MatrixNode matrix || matrix.Rows.Count is < 1 or > 8 || matrix.Rows.Any(row => row.Count != 2)) return false;
        values = new ExactRational[matrix.Rows.Count]; frequencies = new BigInteger[matrix.Rows.Count];
        for (var i = 0; i < matrix.Rows.Count; i++)
        {
            if (!TryReadScalar(matrix.Rows[i][0], out values[i]) || !TryReadInteger(matrix.Rows[i][1], out frequencies[i]) || frequencies[i].Sign < 0) return false;
        }
        return true;
    }
    public static MathematicsVerificationResult CompareSolved(MathematicsSolveResult result, ExactRational expected, string method)
    {
        if (result.Status != MathematicsSolveStatus.Solved || result.ExactResult is null || result.SolutionSet is not FiniteSolutionSet finite || finite.Values.Count != 1)
            return Rejected("Solved result must be represented in ExactResult and a one-value FiniteSolutionSet.");
        if (!TryReadScalar(result.ExactResult, out var actual) || actual != expected
            || !TryReadScalar(finite.Values[0], out var setValue) || setValue != expected)
            return Rejected("Reported result does not equal the independently recomputed value from the original request.");
        return new MathematicsVerificationResult(MathematicsVerificationStatus.Verified,
            [new MathematicsVerificationEvidence(method, "The result was independently recomputed from the original exact probability/statistics input.")], []);
    }
    public static MathematicsVerificationResult Unsupported(string diagnostic) =>
        new(MathematicsVerificationStatus.Unsupported, [], [diagnostic]);
    private static MathematicsVerificationResult Rejected(string diagnostic) =>
        new(MathematicsVerificationStatus.Rejected, [], [diagnostic]);
}
