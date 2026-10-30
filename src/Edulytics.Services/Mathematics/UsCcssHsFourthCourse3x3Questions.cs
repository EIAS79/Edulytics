using System.Globalization;
using System.Numerics;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Bounded exact 3x3 coefficient, inverse and solution Practice.
/// The generator's reference answer uses exact determinants; the independent
/// verifier reconstructs Ax=b or AA^-1=I from parsed rational entries.
/// Human proof of row-reduction operations still requires a teacher rubric.
/// </summary>
public static class UsCcssHsFourthCourse3x3Questions
{
    public const string Coefficients = "usccss.hs.matrix.coefficients_3x3";
    public const string Inverse = "usccss.hs.matrix.inverse_3x3";
    public const string SolveSystem = "usccss.hs.matrix.solve_3x3";
    public const string RowReduce = "usccss.hs.matrix.row_reduce_3x3";

    public static bool Supports(string? family) => family is
        Coefficients or Inverse or SolveSystem or RowReduce;

    public static UsCcssHsFourthCourseQuestions.Item Build(string family, Random random)
    {
        if (!Supports(family))
            throw new ArgumentException("Unsupported 3x3 question family.", nameof(family));

        int[,] a = new int[3, 3];
        var determinant = 0;
        for (var attempt = 0; attempt < 128; attempt++)
        {
            for (var row = 0; row < 3; row++)
                for (var column = 0; column < 3; column++)
                    a[row, column] = random.Next(-3, 4);
            determinant = Determinant(a);
            if (determinant != 0 && Math.Abs(determinant) <= 80)
                break;
        }
        if (determinant == 0 || Math.Abs(determinant) > 80)
            throw new InvalidOperationException("3x3 exact generation exhausted bounded invertible matrices.");

        var x = new[] { random.Next(-4, 5), random.Next(-4, 5), random.Next(-4, 5) };
        var b = new int[3];
        for (var row = 0; row < 3; row++)
            b[row] = Enumerable.Range(0, 3).Sum(column => a[row, column] * x[column]);

        var parameters = new Dictionary<string, int>(StringComparer.Ordinal);
        for (var row = 0; row < 3; row++)
        {
            parameters["b" + row] = b[row];
            for (var col = 0; col < 3; col++)
                parameters[$"a{row}{col}"] = a[row, col];
        }

        var matrix = string.Join(";", Enumerable.Range(0, 3).Select(i =>
            string.Join(",", Enumerable.Range(0, 3).Select(j => a[i, j].ToString(CultureInfo.InvariantCulture)))));
        var rhs = string.Join(",", b);
        var prompt = family switch
        {
            Coefficients => $"For the system {string.Join("; ", Enumerable.Range(0, 3).Select(i => FormatEquation(a, b, i)))}, write its ordered 3x3 coefficient matrix A and column b using variable order x,y,z. Enter a,b,c;d,e,f;g,h,i|u,v,w.",
            Inverse => $"Find the exact inverse of the nonsingular 3x3 coefficient matrix A=[{matrix}]. Enter nine entries row-by-row, with fractions where needed.",
            SolveSystem => $"Solve the exact 3x3 system A[x,y,z]^T=b, where A=[{matrix}] and b=[{rhs}]. Give x,y,z.",
            _ => $"Row-reduce the augmented matrix [A|b] and report the ordered x,y,z for A=[{matrix}], b=[{rhs}]. Explain each row operation separately to your teacher."
        };
        return new UsCcssHsFourthCourseQuestions.Item(
            family, prompt,
            "Verify the inverse by AA^-1=I, or verify the solution by all three original equations. Row-operation proofs are teacher-reviewed.",
            AssessmentItemType.ShortAnswer, parameters);
    }

    // Translate coefficients into source-style equations rather than giving away A and b.
    // Omitted variables are deliberate: learners must supply their zero coefficients.
    private static string FormatEquation(int[,] a, int[] b, int row)
    {
        var variables = new[] { "x", "y", "z" };
        var terms = new List<string>();
        for (var column = 0; column < 3; column++)
        {
            var coefficient = a[row, column];
            if (coefficient == 0)
                continue;
            var magnitude = Math.Abs(coefficient);
            var term = (magnitude == 1 ? "" : magnitude.ToString(CultureInfo.InvariantCulture)) + variables[column];
            terms.Add(terms.Count == 0
                ? (coefficient < 0 ? "-" : "") + term
                : (coefficient < 0 ? " - " : " + ") + term);
        }
        return string.Concat(terms) + " = " + b[row].ToString(CultureInfo.InvariantCulture);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> parameters)
    {
        if (!Supports(family) || !TryLoad(parameters, out var a, out var b))
            throw new ArgumentException("Invalid exact 3x3 parameters.");
        var det = Determinant(a);
        if (det == 0)
            throw new ArgumentException("A singular matrix has no unique inverse.");

        if (family == Coefficients)
        {
            var matrix = string.Join(";", Enumerable.Range(0, 3).Select(i =>
                string.Join(",", Enumerable.Range(0, 3).Select(j => a[i, j]))));
            return matrix + "|" + string.Join(",", b);
        }
        if (family == Inverse)
        {
            // Each column of the inverse solves A v = e_j by Cramer's rule.
            var columns = new string[3, 3];
            for (var column = 0; column < 3; column++)
            {
                var identityColumn = new int[3];
                identityColumn[column] = 1;
                for (var row = 0; row < 3; row++)
                    columns[row, column] = Rational(Determinant(ReplaceColumn(a, row, identityColumn)), det);
            }
            return string.Join(";", Enumerable.Range(0, 3).Select(i =>
                string.Join(",", Enumerable.Range(0, 3).Select(j => columns[i, j]))));
        }
        var solution = new string[3];
        for (var col = 0; col < 3; col++)
            solution[col] = Rational(Determinant(ReplaceColumn(a, col, b)), det);
        return string.Join(",", solution);
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> parameters, string? answer)
    {
        if (!Supports(family) || answer is null || answer.Length > 256 ||
            !TryLoad(parameters, out var a, out var b) || Determinant(a) == 0)
            return false;

        if (family == Coefficients)
        {
            var sides = answer.Split('|');
            if (sides.Length != 2 || !TryReadMatrix(sides[0], 3, 3, out var suppliedA) ||
                !TryReadVector(sides[1], 3, out var suppliedB))
                return false;
            for (var row = 0; row < 3; row++)
            {
                if (suppliedB[row] != Q.Of(b[row]))
                    return false;
                for (var col = 0; col < 3; col++)
                    if (suppliedA[row, col] != Q.Of(a[row, col]))
                        return false;
            }
            return true;
        }

        if (family == Inverse)
        {
            if (!TryReadMatrix(answer, 3, 3, out var candidate))
                return false;
            for (var row = 0; row < 3; row++)
                for (var col = 0; col < 3; col++)
                {
                    var product = Q.Of(0);
                    for (var k = 0; k < 3; k++)
                        product += Q.Of(a[row, k]) * candidate[k, col];
                    if (product != Q.Of(row == col ? 1 : 0))
                        return false;
                }
            return true;
        }

        if (!TryReadVector(answer, 3, out var proposed))
            return false;
        // Independently validate ALL THREE equations, not merely the solver output.
        for (var row = 0; row < 3; row++)
        {
            var sum = Q.Of(0);
            for (var col = 0; col < 3; col++)
                sum += Q.Of(a[row, col]) * proposed[col];
            if (sum != Q.Of(b[row]))
                return false;
        }
        return true;
    }

    private static bool TryLoad(IReadOnlyDictionary<string, int> parameters, out int[,] a, out int[] b)
    {
        a = new int[3, 3]; b = new int[3];
        for (var row = 0; row < 3; row++)
        {
            if (!parameters.TryGetValue("b" + row, out b[row]) || Math.Abs((long)b[row]) > 300)
                return false;
            for (var col = 0; col < 3; col++)
                if (!parameters.TryGetValue($"a{row}{col}", out a[row, col]) ||
                    Math.Abs((long)a[row, col]) > 20)
                    return false;
        }
        return true;
    }

    private static int[,] ReplaceColumn(int[,] a, int target, int[] b)
    {
        var copy = (int[,])a.Clone();
        for (var row = 0; row < 3; row++)
            copy[row, target] = b[row];
        return copy;
    }

    private static int Determinant(int[,] a) => checked(
        a[0, 0] * (a[1, 1] * a[2, 2] - a[1, 2] * a[2, 1]) -
        a[0, 1] * (a[1, 0] * a[2, 2] - a[1, 2] * a[2, 0]) +
        a[0, 2] * (a[1, 0] * a[2, 1] - a[1, 1] * a[2, 0]));

    private static string Rational(int numerator, int denominator)
    {
        var rational = Q.Of(numerator, denominator);
        return rational.Denominator == 1
            ? rational.Numerator.ToString(CultureInfo.InvariantCulture)
            : $"{rational.Numerator}/{rational.Denominator}";
    }

    private static bool TryReadMatrix(string text, int rows, int cols, out Q[,] result)
    {
        result = new Q[rows, cols];
        var lines = text.Trim().Split(';');
        if (lines.Length != rows)
            return false;
        for (var row = 0; row < rows; row++)
            if (!TryReadVector(lines[row], cols, out var values))
                return false;
            else
                for (var col = 0; col < cols; col++)
                    result[row, col] = values[col];
        return true;
    }

    private static bool TryReadVector(string text, int size, out Q[] result)
    {
        result = new Q[size];
        var cells = text.Trim().Split(',');
        if (cells.Length != size)
            return false;
        for (var col = 0; col < size; col++)
            if (!Q.TryParse(cells[col], out result[col]))
                return false;
        return true;
    }

    private readonly record struct Q(BigInteger Numerator, BigInteger Denominator)
    {
        public static Q Of(long numerator, long denominator = 1)
        {
            if (denominator == 0) throw new ArgumentException("Zero denominator.");
            var n = new BigInteger(numerator);
            var d = new BigInteger(denominator);
            if (d < 0) { n = -n; d = -d; }
            var gcd = BigInteger.GreatestCommonDivisor(BigInteger.Abs(n), d);
            return new Q(n / gcd, d / gcd);
        }

        public static Q operator +(Q a, Q b) =>
            Normalize(a.Numerator * b.Denominator + b.Numerator * a.Denominator,
                a.Denominator * b.Denominator);

        public static Q operator *(Q a, Q b) =>
            Normalize(a.Numerator * b.Numerator, a.Denominator * b.Denominator);

        private static Q Normalize(BigInteger numerator, BigInteger denominator)
        {
            var gcd = BigInteger.GreatestCommonDivisor(BigInteger.Abs(numerator), denominator);
            return new Q(numerator / gcd, denominator / gcd);
        }

        public static bool TryParse(string text, out Q parsed)
        {
            parsed = Of(0);
            text = text.Trim();
            if (text.Length is 0 or > 28) return false;
            var halves = text.Split('/');
            if (halves.Length is < 1 or > 2 ||
                !long.TryParse(halves[0], NumberStyles.AllowLeadingSign,
                    CultureInfo.InvariantCulture, out var n) ||
                Math.Abs((decimal)n) > 1_000_000)
                return false;
            var d = 1L;
            if (halves.Length == 2 &&
                (!long.TryParse(halves[1], NumberStyles.AllowLeadingSign,
                    CultureInfo.InvariantCulture, out d) ||
                 d == 0 || Math.Abs((decimal)d) > 1_000_000))
                return false;
            parsed = Of(n, d);
            return true;
        }
    }
}
