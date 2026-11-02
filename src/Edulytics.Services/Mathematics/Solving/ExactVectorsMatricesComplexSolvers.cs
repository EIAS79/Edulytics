using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Solving;

public sealed class ExactVectorAddSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (!ExactLinearAlgebraV2.TryBinaryVectors(request.Problem, "vector_add_exact", out var left, out var right, out var error, out var resourceLimit))
            return ExactLinearAlgebraV2.Failed(request, "vectors-exact-v1", error, resourceLimit);

        var result = new ExactRational[left.Length];
        for (var i = 0; i < left.Length; i++)
        {
            if (!ExactLinearAlgebraArithmetic.TryAdd(left[i], right[i], out result[i], out error))
                return ExactLinearAlgebraV2.ResourceLimit(request, "vectors-exact-v1", error);
        }
        return ExactLinearAlgebraV2.Solved(request, ExactLinearAlgebraV2.ToVector(result), "exact-vector-addition", "vectors-exact-v1");
    }
}

public sealed class ExactVectorDotProductSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (!ExactLinearAlgebraV2.TryBinaryVectors(request.Problem, "vector_dot_exact", out var left, out var right, out var error, out var resourceLimit))
            return ExactLinearAlgebraV2.Failed(request, "vectors-exact-v1", error, resourceLimit);

        var sum = ExactLinearAlgebraV2.Zero;
        for (var i = 0; i < left.Length; i++)
        {
            if (!ExactLinearAlgebraArithmetic.TryMultiply(left[i], right[i], out var term, out error)
                || !ExactLinearAlgebraArithmetic.TryAdd(sum, term, out sum, out error))
                return ExactLinearAlgebraV2.ResourceLimit(request, "vectors-exact-v1", error);
        }
        return ExactLinearAlgebraV2.Solved(request, ExactLinearAlgebraV2.ToNode(sum), "exact-vector-dot-product", "vectors-exact-v1");
    }
}

public sealed class ExactMatrixMultiplySolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Problem is not FunctionCallNode call || call.FunctionName != "matrix_multiply_exact" || call.Arguments.Count != 2)
            return ExactLinearAlgebraV2.Unsupported(request, "matrices-exact-v1", "Exact matrix multiplication requires matrix_multiply_exact with two matrix arguments.");
        if (!ExactLinearAlgebraV2.TryReadMatrix(call.Arguments[0], out var left, out var error, out var resourceLimit)
            || !ExactLinearAlgebraV2.TryReadMatrix(call.Arguments[1], out var right, out error, out resourceLimit))
            return ExactLinearAlgebraV2.Failed(request, "matrices-exact-v1", error, resourceLimit);
        if (left[0].Length != right.Length)
            return ExactLinearAlgebraV2.Unsupported(request, "matrices-exact-v1", "Matrix dimensions are incompatible for multiplication.");

        var rows = new ExactRational[left.Length][];
        for (var r = 0; r < left.Length; r++)
        {
            rows[r] = new ExactRational[right[0].Length];
            for (var c = 0; c < right[0].Length; c++)
            {
                var sum = ExactLinearAlgebraV2.Zero;
                for (var k = 0; k < left[0].Length; k++)
                {
                    if (!ExactLinearAlgebraArithmetic.TryMultiply(left[r][k], right[k][c], out var term, out error)
                        || !ExactLinearAlgebraArithmetic.TryAdd(sum, term, out sum, out error))
                        return ExactLinearAlgebraV2.ResourceLimit(request, "matrices-exact-v1", error);
                }
                rows[r][c] = sum;
            }
        }
        return ExactLinearAlgebraV2.Solved(request, ExactLinearAlgebraV2.ToMatrix(rows), "exact-matrix-multiplication", "matrices-exact-v1");
    }
}

public sealed class ExactMatrixDeterminant2x2Solver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Problem is not FunctionCallNode call || call.FunctionName != "matrix_determinant_2x2_exact" || call.Arguments.Count != 1)
            return ExactLinearAlgebraV2.Unsupported(request, "matrices-exact-v1", "Exact determinant evaluation requires matrix_determinant_2x2_exact with one matrix argument.");
        if (!ExactLinearAlgebraV2.TryReadMatrix(call.Arguments[0], out var matrix, out var error, out var resourceLimit))
            return ExactLinearAlgebraV2.Failed(request, "matrices-exact-v1", error, resourceLimit);
        if (matrix.Length != 2 || matrix[0].Length != 2)
            return ExactLinearAlgebraV2.Unsupported(request, "matrices-exact-v1", "This shadow slice supports determinants for 2x2 matrices only.");

        if (!ExactLinearAlgebraArithmetic.TryMultiply(matrix[0][0], matrix[1][1], out var ad, out error)
            || !ExactLinearAlgebraArithmetic.TryMultiply(matrix[0][1], matrix[1][0], out var bc, out error)
            || !ExactLinearAlgebraArithmetic.TrySubtract(ad, bc, out var determinant, out error))
            return ExactLinearAlgebraV2.ResourceLimit(request, "matrices-exact-v1", error);

        return ExactLinearAlgebraV2.Solved(request, ExactLinearAlgebraV2.ToNode(determinant), "exact-matrix-determinant-2x2", "matrices-exact-v1");
    }
}

public sealed class ExactComplexAddSolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (!ExactLinearAlgebraV2.TryBinaryComplex(request.Problem, "complex_add_exact", out var ar, out var ai, out var br, out var bi, out var error, out var resourceLimit))
            return ExactLinearAlgebraV2.Failed(request, "complex-exact-v1", error, resourceLimit);
        if (!ExactLinearAlgebraArithmetic.TryAdd(ar, br, out var real, out error)
            || !ExactLinearAlgebraArithmetic.TryAdd(ai, bi, out var imaginary, out error))
            return ExactLinearAlgebraV2.ResourceLimit(request, "complex-exact-v1", error);
        return ExactLinearAlgebraV2.Solved(request, ExactLinearAlgebraV2.ToComplex(real, imaginary), "exact-complex-addition", "complex-exact-v1");
    }
}

public sealed class ExactComplexMultiplySolver : IMathematicsSolver
{
    public MathematicsSolveResult Solve(MathematicsSolveRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (!ExactLinearAlgebraV2.TryBinaryComplex(request.Problem, "complex_multiply_exact", out var ar, out var ai, out var br, out var bi, out var error, out var resourceLimit))
            return ExactLinearAlgebraV2.Failed(request, "complex-exact-v1", error, resourceLimit);
        if (!ExactLinearAlgebraArithmetic.TryMultiply(ar, br, out var ac, out error)
            || !ExactLinearAlgebraArithmetic.TryMultiply(ai, bi, out var bd, out error)
            || !ExactLinearAlgebraArithmetic.TrySubtract(ac, bd, out var real, out error)
            || !ExactLinearAlgebraArithmetic.TryMultiply(ar, bi, out var ad, out error)
            || !ExactLinearAlgebraArithmetic.TryMultiply(ai, br, out var bc, out error)
            || !ExactLinearAlgebraArithmetic.TryAdd(ad, bc, out var imaginary, out error))
            return ExactLinearAlgebraV2.ResourceLimit(request, "complex-exact-v1", error);
        return ExactLinearAlgebraV2.Solved(request, ExactLinearAlgebraV2.ToComplex(real, imaginary), "exact-complex-multiplication", "complex-exact-v1");
    }
}

internal static class ExactLinearAlgebraArithmetic
{
    private const int MaxScalarBitLength = 4096;
    private const long MaxIntermediateBitLength = (MaxScalarBitLength * 2L) + 1L;

    public static bool TryAdd(ExactRational left, ExactRational right, out ExactRational result, out string error) => TryAddSubtract(left, right, false, out result, out error);
    public static bool TrySubtract(ExactRational left, ExactRational right, out ExactRational result, out string error) => TryAddSubtract(left, right, true, out result, out error);

    private static bool TryAddSubtract(ExactRational left, ExactRational right, bool subtract, out ExactRational result, out string error)
    {
        result = default;
        error = string.Empty;
        if (!IsValid(left) || !IsValid(right)) { error = "Exact linear-algebra arithmetic received a malformed or oversized scalar."; return false; }
        var leftBits = SaturatingAdd(BitLength(left.Numerator), BitLength(right.Denominator));
        var rightBits = SaturatingAdd(BitLength(right.Numerator), BitLength(left.Denominator));
        if (SaturatingAdd(Math.Max(leftBits, rightBits), 1) > MaxIntermediateBitLength
            || SaturatingAdd(BitLength(left.Denominator), BitLength(right.Denominator)) > MaxIntermediateBitLength)
        { error = "Exact linear-algebra addition/subtraction exceeds the bounded intermediate arithmetic budget."; return false; }
        result = subtract ? left - right : left + right;
        if (!IsValid(result)) { error = "Exact linear-algebra result exceeds the supported 4096-bit scalar budget."; return false; }
        return true;
    }

    public static bool TryMultiply(ExactRational left, ExactRational right, out ExactRational result, out string error)
    {
        result = default;
        error = string.Empty;
        if (!IsValid(left) || !IsValid(right)) { error = "Exact linear-algebra arithmetic received a malformed or oversized scalar."; return false; }
        if (SaturatingAdd(BitLength(left.Numerator), BitLength(right.Numerator)) > MaxIntermediateBitLength
            || SaturatingAdd(BitLength(left.Denominator), BitLength(right.Denominator)) > MaxIntermediateBitLength)
        { error = "Exact linear-algebra multiplication exceeds the bounded intermediate arithmetic budget."; return false; }
        result = left * right;
        if (!IsValid(result)) { error = "Exact linear-algebra result exceeds the supported 4096-bit scalar budget."; return false; }
        return true;
    }

    internal static bool IsValid(ExactRational value) => !value.Denominator.IsZero && BitLength(value.Numerator) <= MaxScalarBitLength && BitLength(value.Denominator) <= MaxScalarBitLength;
    private static long BitLength(BigInteger value) => value.IsZero ? 0 : BigInteger.Abs(value).GetBitLength();
    private static long SaturatingAdd(long left, long right) => left > long.MaxValue - right ? long.MaxValue : left + right;
}

internal static class ExactLinearAlgebraV2
{
    public static readonly ExactRational Zero = new(BigInteger.Zero, BigInteger.One);

    public static bool TryBinaryVectors(MathNode problem, string functionName, out ExactRational[] left, out ExactRational[] right, out string error, out bool resourceLimit)
    {
        left = []; right = []; error = string.Empty; resourceLimit = false;
        if (problem is not FunctionCallNode call || call.FunctionName != functionName || call.Arguments.Count != 2)
        { error = $"{functionName} requires exactly two vector arguments."; return false; }
        if (!TryReadVector(call.Arguments[0], out left, out error, out resourceLimit) || !TryReadVector(call.Arguments[1], out right, out error, out resourceLimit)) return false;
        if (left.Length != right.Length) { error = "Vector dimensions must match."; return false; }
        return true;
    }

    public static bool TryReadVector(MathNode node, out ExactRational[] values, out string error, out bool resourceLimit)
    {
        values = []; error = string.Empty; resourceLimit = false;
        if (node is not VectorNode vector || vector.Components.Count is < 1 or > 4)
        { error = "This shadow slice requires vectors with 1 to 4 exact scalar components."; return false; }
        values = new ExactRational[vector.Components.Count];
        for (var i = 0; i < vector.Components.Count; i++)
            if (!TryReadScalar(vector.Components[i], out values[i], out error, out resourceLimit)) return false;
        return true;
    }

    public static bool TryReadMatrix(MathNode node, out ExactRational[][] rows, out string error, out bool resourceLimit)
    {
        rows = []; error = string.Empty; resourceLimit = false;
        if (node is not MatrixNode matrix || matrix.Rows.Count is < 1 or > 3)
        { error = "This shadow slice requires matrices with 1 to 3 rows."; return false; }
        var columns = matrix.Rows[0].Count;
        if (columns is < 1 or > 3 || matrix.Rows.Any(row => row.Count != columns))
        { error = "Matrices must be rectangular with 1 to 3 columns."; return false; }
        rows = new ExactRational[matrix.Rows.Count][];
        for (var r = 0; r < matrix.Rows.Count; r++)
        {
            rows[r] = new ExactRational[columns];
            for (var c = 0; c < columns; c++)
                if (!TryReadScalar(matrix.Rows[r][c], out rows[r][c], out error, out resourceLimit)) return false;
        }
        return true;
    }

    public static bool TryBinaryComplex(MathNode problem, string functionName, out ExactRational ar, out ExactRational ai, out ExactRational br, out ExactRational bi, out string error, out bool resourceLimit)
    {
        ar = ai = br = bi = default; error = string.Empty; resourceLimit = false;
        if (problem is not FunctionCallNode call || call.FunctionName != functionName || call.Arguments.Count != 2)
        { error = $"{functionName} requires exactly two complex_exact arguments."; return false; }
        return TryReadComplex(call.Arguments[0], out ar, out ai, out error, out resourceLimit)
            && TryReadComplex(call.Arguments[1], out br, out bi, out error, out resourceLimit);
    }

    public static bool TryReadComplex(MathNode node, out ExactRational real, out ExactRational imaginary, out string error, out bool resourceLimit)
    {
        real = imaginary = default; error = string.Empty; resourceLimit = false;
        if (node is not FunctionCallNode complex || complex.FunctionName != "complex_exact" || complex.Arguments.Count != 2)
        { error = "Complex values must use canonical complex_exact(real, imaginary) representation."; return false; }
        return TryReadScalar(complex.Arguments[0], out real, out error, out resourceLimit)
            && TryReadScalar(complex.Arguments[1], out imaginary, out error, out resourceLimit);
    }

    public static bool TryReadScalar(MathNode node, out ExactRational value, out string error, out bool resourceLimit)
    {
        value = default; error = string.Empty; resourceLimit = false;
        switch (node)
        {
            case IntegerNode integer: value = new ExactRational(integer.Value, BigInteger.One); break;
            case RationalNode rational when !rational.Value.Denominator.IsZero: value = rational.Value; break;
            case RationalNode: error = "Exact scalar contains a zero denominator and is malformed."; return false;
            default: error = "Linear-algebra and complex components must be exact integer or rational scalars."; return false;
        }
        if (!ExactLinearAlgebraArithmetic.IsValid(value)) { error = "Exact scalar exceeds the supported 4096-bit arithmetic budget."; resourceLimit = true; return false; }
        return true;
    }

    public static MathNode ToNode(ExactRational value) => value.Denominator == BigInteger.One ? new IntegerNode(value.Numerator) : new RationalNode(value);
    public static VectorNode ToVector(IReadOnlyList<ExactRational> values) => new(values.Select(ToNode).ToArray());
    public static MatrixNode ToMatrix(IReadOnlyList<IReadOnlyList<ExactRational>> rows) => new(rows.Select(row => (IReadOnlyList<MathNode>)row.Select(ToNode).ToArray()).ToArray());
    public static FunctionCallNode ToComplex(ExactRational real, ExactRational imaginary) => new("complex_exact", [ToNode(real), ToNode(imaginary)]);

    public static MathematicsSolveResult Solved(MathematicsSolveRequest request, MathNode result, string strategy, string version) =>
        new(MathematicsSolveStatus.Solved, result, new FiniteSolutionSet([result]), request.Assumptions, strategy, new MathematicsSolutionTrace([]), "edulytics-native-linear-algebra-complex", version, []);
    public static MathematicsSolveResult Failed(MathematicsSolveRequest request, string version, string diagnostic, bool resourceLimit) => resourceLimit ? ResourceLimit(request, version, diagnostic) : Unsupported(request, version, diagnostic);
    public static MathematicsSolveResult Unsupported(MathematicsSolveRequest request, string version, string diagnostic) => new(MathematicsSolveStatus.Unsupported, null, null, request.Assumptions, null, null, "edulytics-native-linear-algebra-complex", version, [diagnostic]);
    public static MathematicsSolveResult ResourceLimit(MathematicsSolveRequest request, string version, string diagnostic) => new(MathematicsSolveStatus.ResourceLimit, null, null, request.Assumptions, null, null, "edulytics-native-linear-algebra-complex", version, [diagnostic]);
}
