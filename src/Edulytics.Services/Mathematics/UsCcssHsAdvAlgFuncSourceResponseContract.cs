using System.Globalization;
using System.Numerics;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Course-isolated, source-bound structured response verifier for Advanced Algebra
/// & Functions. This does NOT register a selected learner Practice family or mark
/// teacher-scored reasoning as machine graded. The shared engine must explicitly
/// adopt a reviewed adapter before any authenticated student use.
/// </summary>
public static class UsCcssHsAdvAlgFuncSourceResponseContract
{
    private const int MaxResponseLength = 1200;

    public static bool RequiresTeacherReview(string? caseId) =>
        caseId is "M05" or "M11" or "L04" or "L05" or "L06" or "L13";

    public static bool IsValidRealLog(double baseValue, double argument) =>
        double.IsFinite(baseValue) && double.IsFinite(argument) &&
        baseValue > 0 && baseValue != 1 && argument > 0;

    /// <summary>For fixed source L06: log_2(x) compared with log_3(x).</summary>
    public static string CrossBaseTwoThreeRelation(double x)
    {
        if (!double.IsFinite(x) || x <= 0) throw new ArgumentOutOfRangeException(nameof(x));
        return x == 1 ? "=" : x < 1 ? "<" : ">";
    }

    /// <summary>
    /// Check ONLY the closed relations in L06; written proof still requires
    /// a separately persisted, teacher-scored rubric.
    /// </summary>
    public static bool VerifyL06ClosedRelations(string? response)
    {
        if (string.IsNullOrWhiteSpace(response) || response.Length > 24) return false;
        var fields = response!.Split(',', StringSplitOptions.TrimEntries);
        return fields.Length == 4 && fields[0] == "<" &&
            fields[1] == CrossBaseTwoThreeRelation(0.5) &&
            fields[2] == CrossBaseTwoThreeRelation(1) &&
            fields[3] == CrossBaseTwoThreeRelation(3);
    }

    /// <summary>
    /// Accept a complete Ax=b representation only: A=[[a,b],[c,d]];b=[e,f].
    /// A single correct row MUST NOT earn full-matrix credit.
    /// </summary>
    public static bool VerifyMatrixEquation(
        string? response, long[,] coefficients, long[] rhs)
    {
        if (!Assignments(response, "A", "b", out var mText, out var bText) ||
            !Matrix(mText, out var matrix) || !Vector(bText, out var vector))
            return false;
        return SameMatrix(matrix, coefficients) && SameVector(vector, rhs);
    }

    /// <summary>Checks a row-level answer, without conflating it with Ax=b.</summary>
    public static bool VerifyCoefficientRow(string? response, long[] row)
    {
        if (!Safe(response) || !Vector(response!, out var actual) ||
            actual.Length != row.Length) return false;
        for (var i = 0; i < row.Length; i++)
            if (actual[i] != Q.From(row[i])) return false;
        return true;
    }

    /// <summary>
    /// Complete source inverse-system answer A^-1=[[...]];x=[...].
    /// Verifies exact matrix identity AND all original equations. A correct
    /// solution vector alone cannot satisfy a task demanding the inverse.
    /// </summary>
    public static bool VerifyInverseAndSolution(
        string? response, long[,] coefficients, long[] rhs)
    {
        if (!Assignments(response, "A^-1", "x", out var inverseText, out var vectorText) ||
            !Matrix(inverseText, out var inverse) || !Vector(vectorText, out var x))
            return false;
        var n = coefficients.GetLength(0);
        if (n != coefficients.GetLength(1) ||
            n != inverse.GetLength(0) || n != inverse.GetLength(1) ||
            n != rhs.Length || n != x.Length || n < 2 || n > 3)
            return false;
        for (var row = 0; row < n; row++)
        {
            var solutionTotal = Q.From(0);
            for (var column = 0; column < n; column++)
                solutionTotal += Q.From(coefficients[row, column]) * x[column];
            if (solutionTotal != Q.From(rhs[row])) return false;
            for (var col = 0; col < n; col++)
            {
                var product = Q.From(0);
                for (var k = 0; k < n; k++)
                    product += Q.From(coefficients[row, k]) * inverse[k, col];
                if (product != Q.From(row == col ? 1 : 0)) return false;
            }
        }
        return true;
    }

    private static bool Safe(string? value) =>
        !string.IsNullOrWhiteSpace(value) &&
        value.Length <= MaxResponseLength &&
        !value.Contains('<') && !value.Contains('>') &&
        !value.Contains('\0');

    private static bool Assignments(
        string? response, string name1, string name2, out string a, out string b)
    {
        a = b = string.Empty;
        if (!Safe(response)) return false;
        var parts = response!.Split(';', StringSplitOptions.None);
        if (parts.Length != 2) return false;
        return Part(parts[0], name1, out a) && Part(parts[1], name2, out b);
    }

    private static bool Part(string item, string name, out string value)
    {
        value = string.Empty;
        var separator = item.IndexOf('=');
        if (separator < 0 || item[..separator].Trim() != name) return false;
        value = item[(separator + 1)..].Trim();
        return value.Length > 0 && !value.Contains('=');
    }

    private static bool Matrix(string input, out Q[,] matrix)
    {
        matrix = new Q[0, 0];
        var t = input.Replace(" ", "", StringComparison.Ordinal);
        if (!t.StartsWith("[[", StringComparison.Ordinal) ||
            !t.EndsWith("]]", StringComparison.Ordinal)) return false;
        var rows = t[2..^2].Split("],[", StringSplitOptions.None);
        if (rows.Length < 2 || rows.Length > 3) return false;
        Q[][] parsed = new Q[rows.Length][];
        for (var r = 0; r < rows.Length; r++)
        {
            if (!ReadList(rows[r], out parsed[r]) ||
                parsed[r].Length != rows.Length) return false;
        }
        matrix = new Q[rows.Length, rows.Length];
        for (var r = 0; r < rows.Length; r++)
            for (var c = 0; c < rows.Length; c++) matrix[r, c] = parsed[r][c];
        return true;
    }

    private static bool Vector(string input, out Q[] values)
    {
        values = [];
        var t = input.Replace(" ", "", StringComparison.Ordinal);
        if (!t.StartsWith("[", StringComparison.Ordinal) ||
            !t.EndsWith("]", StringComparison.Ordinal)) return false;
        return ReadList(t[1..^1], out values);
    }

    private static bool ReadList(string content, out Q[] values)
    {
        values = [];
        var parts = content.Split(',', StringSplitOptions.None);
        if (parts.Length < 2 || parts.Length > 3) return false;
        var parsed = new Q[parts.Length];
        for (var i = 0; i < parts.Length; i++)
            if (!Q.Parse(parts[i], out parsed[i])) return false;
        values = parsed;
        return true;
    }

    private static bool SameMatrix(Q[,] actual, long[,] expected)
    {
        if (actual.GetLength(0) != expected.GetLength(0) ||
            actual.GetLength(1) != expected.GetLength(1)) return false;
        for (var i = 0; i < expected.GetLength(0); i++)
            for (var j = 0; j < expected.GetLength(1); j++)
                if (actual[i, j] != Q.From(expected[i, j])) return false;
        return true;
    }

    private static bool SameVector(Q[] actual, long[] expected)
    {
        if (actual.Length != expected.Length) return false;
        for (var i = 0; i < expected.Length; i++)
            if (actual[i] != Q.From(expected[i])) return false;
        return true;
    }

    private readonly record struct Q(BigInteger Numerator, BigInteger Denominator)
    {
        public static Q From(long n) => new(n, BigInteger.One);
        public static Q operator +(Q x, Q y) => Canonical(
            x.Numerator * y.Denominator + y.Numerator * x.Denominator,
            x.Denominator * y.Denominator);
        public static Q operator *(Q x, Q y) => Canonical(
            x.Numerator * y.Numerator, x.Denominator * y.Denominator);
        private static Q Canonical(BigInteger a, BigInteger b)
        {
            if (b < 0) { a = -a; b = -b; }
            var gcd = BigInteger.GreatestCommonDivisor(BigInteger.Abs(a), b);
            return new Q(a / gcd, b / gcd);
        }
        public static bool Parse(string input, out Q value)
        {
            value = default;
            if (input.Length < 1 || input.Length > 38) return false;
            var chunks = input.Split('/', StringSplitOptions.None);
            if (chunks.Length > 2 ||
                !BigInteger.TryParse(chunks[0], NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var n))
                return false;
            var d = BigInteger.One;
            if (chunks.Length == 2 &&
                !BigInteger.TryParse(chunks[1], NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out d))
                return false;
            if (d.IsZero) return false;
            value = Canonical(n, d);
            return true;
        }
    }
}
