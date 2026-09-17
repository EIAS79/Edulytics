using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Solving;

public sealed class ExactSimpleProbabilitySolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Problem is not FunctionCallNode call || call.FunctionName != "probability_favourable_over_total_exact" || call.Arguments.Count != 2)
            return ExactProbabilityStatisticsV2.Unsupported(request, "probability-statistics-exact-v1", "Exact simple probability requires probability_favourable_over_total_exact(favourable, total).");

        if (!ExactProbabilityStatisticsV2.TryReadInteger(call.Arguments[0], out var favourable, out var error, out var resourceLimit)
            || !ExactProbabilityStatisticsV2.TryReadInteger(call.Arguments[1], out var total, out error, out resourceLimit))
            return ExactProbabilityStatisticsV2.Failed(request, "probability-statistics-exact-v1", error, resourceLimit);

        if (total.Sign <= 0 || favourable.Sign < 0 || favourable > total)
            return ExactProbabilityStatisticsV2.Unsupported(request, "probability-statistics-exact-v1", "Simple probability counts require 0 <= favourable <= total and total > 0.");

        var numerator = new ExactRational(favourable, BigInteger.One);
        var denominator = new ExactRational(total, BigInteger.One);
        if (!ExactProbabilityStatisticsArithmetic.TryDivide(numerator, denominator, out var probability, out error))
            return ExactProbabilityStatisticsV2.ResourceLimit(request, "probability-statistics-exact-v1", error);

        return ExactProbabilityStatisticsV2.Solved(request, ExactProbabilityStatisticsV2.ToNode(probability), "exact-favourable-over-total-probability", "probability-statistics-exact-v1");
    }
}

public sealed class ExactProbabilityComplementSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Problem is not FunctionCallNode call || call.FunctionName != "probability_complement_exact" || call.Arguments.Count != 1)
            return ExactProbabilityStatisticsV2.Unsupported(request, "probability-statistics-exact-v1", "Exact complement probability requires probability_complement_exact(p).");

        if (!ExactProbabilityStatisticsV2.TryReadProbability(call.Arguments[0], out var probability, out var error, out var resourceLimit))
            return ExactProbabilityStatisticsV2.Failed(request, "probability-statistics-exact-v1", error, resourceLimit);

        if (!ExactProbabilityStatisticsArithmetic.TrySubtract(ExactProbabilityStatisticsV2.One, probability, out var complement, out error))
            return ExactProbabilityStatisticsV2.ResourceLimit(request, "probability-statistics-exact-v1", error);

        return ExactProbabilityStatisticsV2.Solved(request, ExactProbabilityStatisticsV2.ToNode(complement), "exact-probability-complement", "probability-statistics-exact-v1");
    }
}

public sealed class ExactArithmeticMeanSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Problem is not FunctionCallNode call || call.FunctionName != "statistics_mean_exact" || call.Arguments.Count != 1)
            return ExactProbabilityStatisticsV2.Unsupported(request, "probability-statistics-exact-v1", "Exact arithmetic mean requires statistics_mean_exact(vector).");

        if (!ExactProbabilityStatisticsV2.TryReadDataVector(call.Arguments[0], out var values, out var error, out var resourceLimit))
            return ExactProbabilityStatisticsV2.Failed(request, "probability-statistics-exact-v1", error, resourceLimit);

        var sum = ExactProbabilityStatisticsV2.Zero;
        foreach (var value in values)
            if (!ExactProbabilityStatisticsArithmetic.TryAdd(sum, value, out sum, out error))
                return ExactProbabilityStatisticsV2.ResourceLimit(request, "probability-statistics-exact-v1", error);

        var count = new ExactRational(new BigInteger(values.Length), BigInteger.One);
        if (!ExactProbabilityStatisticsArithmetic.TryDivide(sum, count, out var mean, out error))
            return ExactProbabilityStatisticsV2.ResourceLimit(request, "probability-statistics-exact-v1", error);

        return ExactProbabilityStatisticsV2.Solved(request, ExactProbabilityStatisticsV2.ToNode(mean), "exact-arithmetic-mean", "probability-statistics-exact-v1");
    }
}

public sealed class ExactFrequencyMeanSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Problem is not FunctionCallNode call || call.FunctionName != "statistics_frequency_mean_exact" || call.Arguments.Count != 1)
            return ExactProbabilityStatisticsV2.Unsupported(request, "probability-statistics-exact-v1", "Exact frequency mean requires statistics_frequency_mean_exact(matrix), with rows [value, frequency].");

        if (!ExactProbabilityStatisticsV2.TryReadFrequencyTable(call.Arguments[0], out var values, out var frequencies, out var error, out var resourceLimit))
            return ExactProbabilityStatisticsV2.Failed(request, "probability-statistics-exact-v1", error, resourceLimit);

        var weightedSum = ExactProbabilityStatisticsV2.Zero;
        var totalFrequency = BigInteger.Zero;
        for (var i = 0; i < values.Length; i++)
        {
            if (!ExactProbabilityStatisticsArithmetic.TryMultiply(values[i], new ExactRational(frequencies[i], BigInteger.One), out var weighted, out error)
                || !ExactProbabilityStatisticsArithmetic.TryAdd(weightedSum, weighted, out weightedSum, out error))
                return ExactProbabilityStatisticsV2.ResourceLimit(request, "probability-statistics-exact-v1", error);
            totalFrequency += frequencies[i];
        }

        if (totalFrequency.Sign <= 0)
            return ExactProbabilityStatisticsV2.Unsupported(request, "probability-statistics-exact-v1", "A frequency table must contain at least one positive frequency.");

        var total = new ExactRational(totalFrequency, BigInteger.One);
        if (!ExactProbabilityStatisticsArithmetic.TryDivide(weightedSum, total, out var mean, out error))
            return ExactProbabilityStatisticsV2.ResourceLimit(request, "probability-statistics-exact-v1", error);

        return ExactProbabilityStatisticsV2.Solved(request, ExactProbabilityStatisticsV2.ToNode(mean), "exact-frequency-table-mean", "probability-statistics-exact-v1");
    }
}

internal static class ExactProbabilityStatisticsArithmetic
{
    private const int MaxScalarBitLength = 4096;
    private const long MaxIntermediateBitLength = (MaxScalarBitLength * 2L) + 1L;

    public static bool TryAdd(ExactRational left, ExactRational right, out ExactRational result, out string error) => TryAddSubtract(left, right, false, out result, out error);
    public static bool TrySubtract(ExactRational left, ExactRational right, out ExactRational result, out string error) => TryAddSubtract(left, right, true, out result, out error);

    private static bool TryAddSubtract(ExactRational left, ExactRational right, bool subtract, out ExactRational result, out string error)
    {
        result = default;
        error = string.Empty;
        if (!IsValid(left) || !IsValid(right)) { error = "Probability/statistics arithmetic received a malformed or oversized scalar."; return false; }
        var leftBits = SaturatingAdd(BitLength(left.Numerator), BitLength(right.Denominator));
        var rightBits = SaturatingAdd(BitLength(right.Numerator), BitLength(left.Denominator));
        if (SaturatingAdd(Math.Max(leftBits, rightBits), 1) > MaxIntermediateBitLength
            || SaturatingAdd(BitLength(left.Denominator), BitLength(right.Denominator)) > MaxIntermediateBitLength)
        { error = "Probability/statistics addition or subtraction exceeds the bounded intermediate arithmetic budget."; return false; }
        result = subtract ? left - right : left + right;
        if (!IsValid(result)) { error = "Probability/statistics result exceeds the supported 4096-bit scalar budget."; return false; }
        return true;
    }

    public static bool TryMultiply(ExactRational left, ExactRational right, out ExactRational result, out string error)
    {
        result = default;
        error = string.Empty;
        if (!IsValid(left) || !IsValid(right)) { error = "Probability/statistics arithmetic received a malformed or oversized scalar."; return false; }
        if (SaturatingAdd(BitLength(left.Numerator), BitLength(right.Numerator)) > MaxIntermediateBitLength
            || SaturatingAdd(BitLength(left.Denominator), BitLength(right.Denominator)) > MaxIntermediateBitLength)
        { error = "Probability/statistics multiplication exceeds the bounded intermediate arithmetic budget."; return false; }
        result = left * right;
        if (!IsValid(result)) { error = "Probability/statistics result exceeds the supported 4096-bit scalar budget."; return false; }
        return true;
    }

    public static bool TryDivide(ExactRational left, ExactRational right, out ExactRational result, out string error)
    {
        result = default;
        error = string.Empty;
        if (!IsValid(left) || !IsValid(right)) { error = "Probability/statistics arithmetic received a malformed or oversized scalar."; return false; }
        if (right.Numerator.IsZero) { error = "Probability/statistics division by zero is unsupported."; return false; }
        if (SaturatingAdd(BitLength(left.Numerator), BitLength(right.Denominator)) > MaxIntermediateBitLength
            || SaturatingAdd(BitLength(left.Denominator), BitLength(right.Numerator)) > MaxIntermediateBitLength)
        { error = "Probability/statistics division exceeds the bounded intermediate arithmetic budget."; return false; }
        result = left / right;
        if (!IsValid(result)) { error = "Probability/statistics result exceeds the supported 4096-bit scalar budget."; return false; }
        return true;
    }

    internal static bool IsValid(ExactRational value) => !value.Denominator.IsZero && BitLength(value.Numerator) <= MaxScalarBitLength && BitLength(value.Denominator) <= MaxScalarBitLength;
    private static long BitLength(BigInteger value) => value.IsZero ? 0 : BigInteger.Abs(value).GetBitLength();
    private static long SaturatingAdd(long left, long right) => left > long.MaxValue - right ? long.MaxValue : left + right;
}

internal static class ExactProbabilityStatisticsV2
{
    public static readonly ExactRational Zero = new(BigInteger.Zero, BigInteger.One);
    public static readonly ExactRational One = new(BigInteger.One, BigInteger.One);

    public static bool TryReadInteger(MathNode node, out BigInteger value, out string error, out bool resourceLimit)
    {
        value = default;
        error = string.Empty;
        resourceLimit = false;
        if (node is not IntegerNode integer) { error = "This operation requires an exact integer count."; return false; }
        var rational = new ExactRational(integer.Value, BigInteger.One);
        if (!ExactProbabilityStatisticsArithmetic.IsValid(rational)) { error = "Integer count exceeds the supported 4096-bit arithmetic budget."; resourceLimit = true; return false; }
        value = integer.Value;
        return true;
    }

    public static bool TryReadScalar(MathNode node, out ExactRational value, out string error, out bool resourceLimit)
    {
        value = default;
        error = string.Empty;
        resourceLimit = false;
        switch (node)
        {
            case IntegerNode integer: value = new ExactRational(integer.Value, BigInteger.One); break;
            case RationalNode rational when !rational.Value.Denominator.IsZero: value = rational.Value; break;
            case RationalNode: error = "Exact scalar contains a zero denominator and is malformed."; return false;
            default: error = "This probability/statistics slice accepts exact integer or rational scalars only."; return false;
        }
        if (!ExactProbabilityStatisticsArithmetic.IsValid(value)) { error = "Exact scalar exceeds the supported 4096-bit arithmetic budget."; resourceLimit = true; return false; }
        return true;
    }

    public static bool TryReadProbability(MathNode node, out ExactRational value, out string error, out bool resourceLimit)
    {
        if (!TryReadScalar(node, out value, out error, out resourceLimit)) return false;
        if (value.CompareTo(Zero) < 0 || value.CompareTo(One) > 0) { error = "Probability values must lie in the closed interval [0, 1]."; return false; }
        return true;
    }

    public static bool TryReadDataVector(MathNode node, out ExactRational[] values, out string error, out bool resourceLimit)
    {
        values = [];
        error = string.Empty;
        resourceLimit = false;
        if (node is not VectorNode vector || vector.Components.Count is < 1 or > 8)
        { error = "Exact mean currently supports data vectors containing 1 to 8 values."; return false; }
        values = new ExactRational[vector.Components.Count];
        for (var i = 0; i < vector.Components.Count; i++)
            if (!TryReadScalar(vector.Components[i], out values[i], out error, out resourceLimit)) return false;
        return true;
    }

    public static bool TryReadFrequencyTable(MathNode node, out ExactRational[] values, out BigInteger[] frequencies, out string error, out bool resourceLimit)
    {
        values = [];
        frequencies = [];
        error = string.Empty;
        resourceLimit = false;
        if (node is not MatrixNode matrix || matrix.Rows.Count is < 1 or > 8 || matrix.Rows.Any(row => row.Count != 2))
        { error = "Frequency mean requires 1 to 8 rows, each exactly [value, frequency]."; return false; }
        values = new ExactRational[matrix.Rows.Count];
        frequencies = new BigInteger[matrix.Rows.Count];
        for (var i = 0; i < matrix.Rows.Count; i++)
        {
            if (!TryReadScalar(matrix.Rows[i][0], out values[i], out error, out resourceLimit)) return false;
            if (!TryReadInteger(matrix.Rows[i][1], out frequencies[i], out error, out resourceLimit)) return false;
            if (frequencies[i].Sign < 0) { error = "Frequencies must be non-negative integers."; return false; }
        }
        return true;
    }

    public static MathNode ToNode(ExactRational value) => value.Denominator == BigInteger.One ? new IntegerNode(value.Numerator) : new RationalNode(value);

    public static MathematicsSolveResult Solved(MathematicsSolveRequest request, MathNode result, string strategy, string version) =>
        new(MathematicsSolveStatus.Solved, result, new FiniteSolutionSet([result]), request.Assumptions, strategy, new MathematicsSolutionTrace([]), "edulytics-native-probability-statistics", version, []);
    public static MathematicsSolveResult Failed(MathematicsSolveRequest request, string version, string diagnostic, bool resourceLimit) => resourceLimit ? ResourceLimit(request, version, diagnostic) : Unsupported(request, version, diagnostic);
    public static MathematicsSolveResult Unsupported(MathematicsSolveRequest request, string version, string diagnostic) =>
        new(MathematicsSolveStatus.Unsupported, null, null, request.Assumptions, null, null, "edulytics-native-probability-statistics", version, [diagnostic]);
    public static MathematicsSolveResult ResourceLimit(MathematicsSolveRequest request, string version, string diagnostic) =>
        new(MathematicsSolveStatus.ResourceLimit, null, null, request.Assumptions, null, null, "edulytics-native-probability-statistics", version, [diagnostic]);
}
