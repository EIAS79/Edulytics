using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Core.Mathematics.Verification;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Verification;

public sealed class ExactVectorAddVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request); ArgumentNullException.ThrowIfNull(result);
        if (!VerifierLinearAlgebraV2.TryBinaryVectors(request.Problem, "vector_add_exact", out var left, out var right, out var error)) return VerifierLinearAlgebraV2.Unsupported(error);
        var expected = new ExactRational[left.Length];
        for (var i = 0; i < left.Length; i++) if (!VerifierLinearAlgebraArithmetic.TryAdd(left[i], right[i], out expected[i], out error)) return VerifierLinearAlgebraV2.Unsupported(error);
        return VerifierLinearAlgebraV2.CompareSolved(result, VerifierLinearAlgebraV2.ToVector(expected), "independent-exact-vector-addition");
    }
}

public sealed class ExactVectorDotProductVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request); ArgumentNullException.ThrowIfNull(result);
        if (!VerifierLinearAlgebraV2.TryBinaryVectors(request.Problem, "vector_dot_exact", out var left, out var right, out var error)) return VerifierLinearAlgebraV2.Unsupported(error);
        var sum = VerifierLinearAlgebraV2.Zero;
        for (var i = 0; i < left.Length; i++)
        {
            if (!VerifierLinearAlgebraArithmetic.TryMultiply(left[i], right[i], out var term, out error)
                || !VerifierLinearAlgebraArithmetic.TryAdd(sum, term, out sum, out error)) return VerifierLinearAlgebraV2.Unsupported(error);
        }
        return VerifierLinearAlgebraV2.CompareSolved(result, VerifierLinearAlgebraV2.ToNode(sum), "independent-exact-vector-dot-product");
    }
}

public sealed class ExactMatrixMultiplyVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request); ArgumentNullException.ThrowIfNull(result);
        if (request.Problem is not FunctionCallNode call || call.FunctionName != "matrix_multiply_exact" || call.Arguments.Count != 2) return VerifierLinearAlgebraV2.Unsupported("Independent matrix verification requires matrix_multiply_exact with two matrix arguments.");
        if (!VerifierLinearAlgebraV2.TryReadMatrix(call.Arguments[0], out var left, out var error)
            || !VerifierLinearAlgebraV2.TryReadMatrix(call.Arguments[1], out var right, out error)) return VerifierLinearAlgebraV2.Unsupported(error);
        if (left[0].Length != right.Length) return VerifierLinearAlgebraV2.Unsupported("Matrix dimensions are incompatible for multiplication.");
        var rows = new ExactRational[left.Length][];
        for (var r = 0; r < left.Length; r++)
        {
            rows[r] = new ExactRational[right[0].Length];
            for (var c = 0; c < right[0].Length; c++)
            {
                var sum = VerifierLinearAlgebraV2.Zero;
                for (var k = 0; k < left[0].Length; k++)
                {
                    if (!VerifierLinearAlgebraArithmetic.TryMultiply(left[r][k], right[k][c], out var term, out error)
                        || !VerifierLinearAlgebraArithmetic.TryAdd(sum, term, out sum, out error)) return VerifierLinearAlgebraV2.Unsupported(error);
                }
                rows[r][c] = sum;
            }
        }
        return VerifierLinearAlgebraV2.CompareSolved(result, VerifierLinearAlgebraV2.ToMatrix(rows), "independent-exact-matrix-multiplication");
    }
}

public sealed class ExactMatrixDeterminant2x2Verifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request); ArgumentNullException.ThrowIfNull(result);
        if (request.Problem is not FunctionCallNode call || call.FunctionName != "matrix_determinant_2x2_exact" || call.Arguments.Count != 1) return VerifierLinearAlgebraV2.Unsupported("Independent determinant verification requires matrix_determinant_2x2_exact with one matrix argument.");
        if (!VerifierLinearAlgebraV2.TryReadMatrix(call.Arguments[0], out var matrix, out var error)) return VerifierLinearAlgebraV2.Unsupported(error);
        if (matrix.Length != 2 || matrix[0].Length != 2) return VerifierLinearAlgebraV2.Unsupported("Independent determinant verification supports 2x2 matrices only.");
        if (!VerifierLinearAlgebraArithmetic.TryMultiply(matrix[0][0], matrix[1][1], out var ad, out error)
            || !VerifierLinearAlgebraArithmetic.TryMultiply(matrix[0][1], matrix[1][0], out var bc, out error)
            || !VerifierLinearAlgebraArithmetic.TrySubtract(ad, bc, out var expected, out error)) return VerifierLinearAlgebraV2.Unsupported(error);
        return VerifierLinearAlgebraV2.CompareSolved(result, VerifierLinearAlgebraV2.ToNode(expected), "independent-exact-matrix-determinant-2x2");
    }
}

public sealed class ExactComplexAddVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request); ArgumentNullException.ThrowIfNull(result);
        if (!VerifierLinearAlgebraV2.TryBinaryComplex(request.Problem, "complex_add_exact", out var ar, out var ai, out var br, out var bi, out var error)) return VerifierLinearAlgebraV2.Unsupported(error);
        if (!VerifierLinearAlgebraArithmetic.TryAdd(ar, br, out var real, out error)
            || !VerifierLinearAlgebraArithmetic.TryAdd(ai, bi, out var imaginary, out error)) return VerifierLinearAlgebraV2.Unsupported(error);
        return VerifierLinearAlgebraV2.CompareSolved(result, VerifierLinearAlgebraV2.ToComplex(real, imaginary), "independent-exact-complex-addition");
    }
}

public sealed class ExactComplexMultiplyVerifier : IMathematicsVerifier
{
    public MathematicsVerificationResult Verify(MathematicsSolveRequest request, MathematicsSolveResult result)
    {
        ArgumentNullException.ThrowIfNull(request); ArgumentNullException.ThrowIfNull(result);
        if (!VerifierLinearAlgebraV2.TryBinaryComplex(request.Problem, "complex_multiply_exact", out var ar, out var ai, out var br, out var bi, out var error)) return VerifierLinearAlgebraV2.Unsupported(error);
        if (!VerifierLinearAlgebraArithmetic.TryMultiply(ar, br, out var ac, out error)
            || !VerifierLinearAlgebraArithmetic.TryMultiply(ai, bi, out var bd, out error)
            || !VerifierLinearAlgebraArithmetic.TrySubtract(ac, bd, out var real, out error)
            || !VerifierLinearAlgebraArithmetic.TryMultiply(ar, bi, out var ad, out error)
            || !VerifierLinearAlgebraArithmetic.TryMultiply(ai, br, out var bc, out error)
            || !VerifierLinearAlgebraArithmetic.TryAdd(ad, bc, out var imaginary, out error)) return VerifierLinearAlgebraV2.Unsupported(error);
        return VerifierLinearAlgebraV2.CompareSolved(result, VerifierLinearAlgebraV2.ToComplex(real, imaginary), "independent-exact-complex-multiplication");
    }
}

internal static class VerifierLinearAlgebraArithmetic
{
    private const int MaxScalarBitLength = 4096;
    private const long MaxIntermediateBitLength = (MaxScalarBitLength * 2L) + 1L;
    public static bool TryAdd(ExactRational left, ExactRational right, out ExactRational result, out string error) => TryAddSubtract(left, right, false, out result, out error);
    public static bool TrySubtract(ExactRational left, ExactRational right, out ExactRational result, out string error) => TryAddSubtract(left, right, true, out result, out error);
    private static bool TryAddSubtract(ExactRational left, ExactRational right, bool subtract, out ExactRational result, out string error)
    {
        result = default; error = string.Empty;
        if (!IsValid(left) || !IsValid(right)) { error = "Independent linear-algebra verification received a malformed or oversized scalar."; return false; }
        var leftBits = SaturatingAdd(BitLength(left.Numerator), BitLength(right.Denominator));
        var rightBits = SaturatingAdd(BitLength(right.Numerator), BitLength(left.Denominator));
        if (SaturatingAdd(Math.Max(leftBits, rightBits), 1) > MaxIntermediateBitLength || SaturatingAdd(BitLength(left.Denominator), BitLength(right.Denominator)) > MaxIntermediateBitLength)
        { error = "Independent addition/subtraction exceeds its bounded intermediate arithmetic budget."; return false; }
        result = subtract ? left - right : left + right;
        if (!IsValid(result)) { error = "Independent result exceeds the 4096-bit scalar budget."; return false; }
        return true;
    }
    public static bool TryMultiply(ExactRational left, ExactRational right, out ExactRational result, out string error)
    {
        result = default; error = string.Empty;
        if (!IsValid(left) || !IsValid(right)) { error = "Independent linear-algebra verification received a malformed or oversized scalar."; return false; }
        if (SaturatingAdd(BitLength(left.Numerator), BitLength(right.Numerator)) > MaxIntermediateBitLength || SaturatingAdd(BitLength(left.Denominator), BitLength(right.Denominator)) > MaxIntermediateBitLength)
        { error = "Independent multiplication exceeds its bounded intermediate arithmetic budget."; return false; }
        result = left * right;
        if (!IsValid(result)) { error = "Independent result exceeds the 4096-bit scalar budget."; return false; }
        return true;
    }
    internal static bool IsValid(ExactRational value) => !value.Denominator.IsZero && BitLength(value.Numerator) <= MaxScalarBitLength && BitLength(value.Denominator) <= MaxScalarBitLength;
    private static long BitLength(BigInteger value) => value.IsZero ? 0 : BigInteger.Abs(value).GetBitLength();
    private static long SaturatingAdd(long left, long right) => left > long.MaxValue - right ? long.MaxValue : left + right;
}

internal static class VerifierLinearAlgebraV2
{
    public static readonly ExactRational Zero = new(BigInteger.Zero, BigInteger.One);
    public static bool TryBinaryVectors(MathNode problem, string functionName, out ExactRational[] left, out ExactRational[] right, out string error)
    {
        left = []; right = []; error = string.Empty;
        if (problem is not FunctionCallNode call || call.FunctionName != functionName || call.Arguments.Count != 2) { error = $"Independent verification requires {functionName} with two vectors."; return false; }
        if (!TryReadVector(call.Arguments[0], out left, out error) || !TryReadVector(call.Arguments[1], out right, out error)) return false;
        if (left.Length != right.Length) { error = "Independent verification requires matching vector dimensions."; return false; }
        return true;
    }
    public static bool TryReadVector(MathNode node, out ExactRational[] values, out string error)
    {
        values = []; error = string.Empty;
        if (node is not VectorNode vector || vector.Components.Count is < 1 or > 4) { error = "Independent verification supports vectors with 1 to 4 exact components."; return false; }
        values = new ExactRational[vector.Components.Count];
        for (var i = 0; i < vector.Components.Count; i++) if (!TryReadScalar(vector.Components[i], out values[i])) { error = "Independent vector verification requires bounded exact scalar components."; return false; }
        return true;
    }
    public static bool TryReadMatrix(MathNode node, out ExactRational[][] rows, out string error)
    {
        rows = []; error = string.Empty;
        if (node is not MatrixNode matrix || matrix.Rows.Count is < 1 or > 3) { error = "Independent verification supports matrices with 1 to 3 rows."; return false; }
        var columns = matrix.Rows[0].Count;
        if (columns is < 1 or > 3 || matrix.Rows.Any(row => row.Count != columns)) { error = "Independent verification requires rectangular matrices with 1 to 3 columns."; return false; }
        rows = new ExactRational[matrix.Rows.Count][];
        for (var r = 0; r < matrix.Rows.Count; r++)
        {
            rows[r] = new ExactRational[columns];
            for (var c = 0; c < columns; c++) if (!TryReadScalar(matrix.Rows[r][c], out rows[r][c])) { error = "Independent matrix verification requires bounded exact scalar cells."; return false; }
        }
        return true;
    }
    public static bool TryBinaryComplex(MathNode problem, string functionName, out ExactRational ar, out ExactRational ai, out ExactRational br, out ExactRational bi, out string error)
    {
        ar = ai = br = bi = default; error = string.Empty;
        if (problem is not FunctionCallNode call || call.FunctionName != functionName || call.Arguments.Count != 2) { error = $"Independent verification requires {functionName} with two complex_exact values."; return false; }
        return TryReadComplex(call.Arguments[0], out ar, out ai, out error) && TryReadComplex(call.Arguments[1], out br, out bi, out error);
    }
    public static bool TryReadComplex(MathNode node, out ExactRational real, out ExactRational imaginary, out string error)
    {
        real = imaginary = default; error = string.Empty;
        if (node is not FunctionCallNode complex || complex.FunctionName != "complex_exact" || complex.Arguments.Count != 2) { error = "Independent verification requires canonical complex_exact(real, imaginary) representation."; return false; }
        if (!TryReadScalar(complex.Arguments[0], out real) || !TryReadScalar(complex.Arguments[1], out imaginary)) { error = "Independent complex verification requires bounded exact scalar parts."; return false; }
        return true;
    }
    private static bool TryReadScalar(MathNode node, out ExactRational value)
    {
        switch (node)
        {
            case IntegerNode integer: value = new ExactRational(integer.Value, BigInteger.One); return VerifierLinearAlgebraArithmetic.IsValid(value);
            case RationalNode rational when !rational.Value.Denominator.IsZero: value = rational.Value; return VerifierLinearAlgebraArithmetic.IsValid(value);
            default: value = default; return false;
        }
    }
    public static MathNode ToNode(ExactRational value) => value.Denominator == BigInteger.One ? new IntegerNode(value.Numerator) : new RationalNode(value);
    public static VectorNode ToVector(IReadOnlyList<ExactRational> values) => new(values.Select(ToNode).ToArray());
    public static MatrixNode ToMatrix(IReadOnlyList<IReadOnlyList<ExactRational>> rows) => new(rows.Select(row => (IReadOnlyList<MathNode>)row.Select(ToNode).ToArray()).ToArray());
    public static FunctionCallNode ToComplex(ExactRational real, ExactRational imaginary) => new("complex_exact", [ToNode(real), ToNode(imaginary)]);

    public static MathematicsVerificationResult CompareSolved(MathematicsSolveResult result, MathNode expected, string method)
    {
        if (result.Status != MathematicsSolveStatus.Solved || result.ExactResult is null || result.SolutionSet is not FiniteSolutionSet finite || finite.Values.Count != 1)
            return Rejected("Solved result must be represented in ExactResult and a one-value FiniteSolutionSet.");
        if (!Equivalent(result.ExactResult, expected) || !Equivalent(finite.Values[0], expected))
            return Rejected("Reported result does not equal the independently recomputed value from the original request.");
        return new MathematicsVerificationResult(MathematicsVerificationStatus.Verified, [new MathematicsVerificationEvidence(method, "The result was independently recomputed from the original exact input.")], []);
    }

    private static bool Equivalent(MathNode actual, MathNode expected)
    {
        if (TryReadScalar(actual, out var aScalar) && TryReadScalar(expected, out var eScalar)) return aScalar == eScalar;
        if (actual is VectorNode av && expected is VectorNode ev)
        {
            if (!TryReadVector(av, out var a, out _) || !TryReadVector(ev, out var e, out _) || a.Length != e.Length) return false;
            return a.SequenceEqual(e);
        }
        if (actual is MatrixNode am && expected is MatrixNode em)
        {
            if (!TryReadMatrix(am, out var a, out _) || !TryReadMatrix(em, out var e, out _) || a.Length != e.Length || a[0].Length != e[0].Length) return false;
            for (var r = 0; r < a.Length; r++) if (!a[r].SequenceEqual(e[r])) return false;
            return true;
        }
        if (TryReadComplex(actual, out var ar, out var ai, out _) && TryReadComplex(expected, out var er, out var ei, out _)) return ar == er && ai == ei;
        return false;
    }
    private static MathematicsVerificationResult Rejected(string diagnostic) => new(MathematicsVerificationStatus.Rejected, [], [diagnostic]);
    public static MathematicsVerificationResult Unsupported(string diagnostic) => new(MathematicsVerificationStatus.Unsupported, [], [diagnostic]);
}
