using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

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

    public enum MatrixCaseDisposition
    {
        IncorrectOrMalformed,
        CorrectClosedAnswer,
        TeacherReviewRequired
    }

    /// <summary>
    /// Source-case bridge for the existing M01-M11 lesson fixtures.
    /// Never assign automated marks to free explanations or proof claims.
    /// This is a course-only verifier/teacher-routing contract, NOT a
    /// global Practice registration, selected runtime, or teacher workflow.
    /// </summary>
    public static MatrixCaseDisposition AssessL01MatrixCase(string? caseId, string? response)
    {
        // M11's original row-operation notation uses "<-". Allow exactly
        // that mathematical arrow in human-reviewed prose, never HTML tags.
        if (caseId is "M05" or "M11")
            return SafeTeacherExplanation(response)
                ? MatrixCaseDisposition.TeacherReviewRequired
                : MatrixCaseDisposition.IncorrectOrMalformed;

        if (!Safe(response)) return MatrixCaseDisposition.IncorrectOrMalformed;

        // The source fixture permits a prose Ax=b explanation (M01) and
        // determinant+factored-inverse display (M03); the exact structured
        // parser must not reject those as mathematically wrong OR auto-award
        // proof/completeness credit. A teacher evaluates these representations.
        if (caseId == "M01" && response!.TrimStart().StartsWith(
                "Ax=b with", StringComparison.OrdinalIgnoreCase))
            return MatrixCaseDisposition.TeacherReviewRequired;
        if (caseId == "M03" && response!.TrimStart().StartsWith(
                "det(A)=7;", StringComparison.OrdinalIgnoreCase))
            return MatrixCaseDisposition.TeacherReviewRequired;

        var valid = caseId switch
        {
            "M01" => VerifyMatrixEquation(response, new long[,]
                { { 1, 1, 1 }, { 2, -1, 1 }, { 1, 2, -1 } }, [6, 3, 2]),
            "M02" => VerifyInverseAndSolution(response, new long[,]
                { { 2, 1 }, { 1, 1 } }, [5, 3]),
            "M03" => VerifyInverseAndSolution(response, new long[,]
                { { 1, 1, 1 }, { 2, -1, 1 }, { 1, 2, -1 } }, [6, 3, 2]),
            "M04" => VerifySystemSolution(response, new long[,]
                { { 1, 1, 1 }, { 2, -1, 1 }, { 1, 2, -1 } }, [6, 3, 2]),
            "M06" => VerifySystemClassification(response, new long[,]
                { { 1, 1 }, { 2, 2 } }, [2, 5]),
            "M07" => VerifyInverseEligibility(response, new long[,]
                { { 1, 2 }, { 2, 4 } }),
            "M08" => VerifyInverseEligibility(response, new long[,]
                { { 2, 1 }, { 1, 1 } }, rhsLength: 3),
            "M09" => VerifyCoefficientRow(response, [2, -1, 1]),
            "M10" => VerifyInverseEligibility(response, new long[,]
                { { 1, 2, 3 }, { 4, 5, 6 } }),
            _ => false
        };
        return valid
            ? MatrixCaseDisposition.CorrectClosedAnswer
            : MatrixCaseDisposition.IncorrectOrMalformed;
    }

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

    /// <summary>
    /// The original MVP 10.6H task 1a/1b asks for the entire 3x3 product:
    /// I*A=A and the supplied A^-1*A=I. Check a complete result only;
    /// students' explanation of the identity/inverse property is teacher-scored.
    /// This source oracle does not register a learner-facing Practice route.
    /// </summary>
    public static bool VerifySourceMatrixProduct(
        string? response, long[,] coefficients, bool inverseTimesCoefficients)
    {
        if (!Safe(response) || !Matrix(response!, out var actual) ||
            coefficients.GetLength(0) != 3 || coefficients.GetLength(1) != 3 ||
            actual.GetLength(0) != 3 || actual.GetLength(1) != 3)
            return false;
        for (var row = 0; row < 3; row++)
            for (var col = 0; col < 3; col++)
            {
                var expected = inverseTimesCoefficients
                    ? row == col ? 1L : 0L
                    : coefficients[row, col];
                if (actual[row, col] != Q.From(expected)) return false;
            }
        return true;
    }

    /// <summary>Checks a row-level answer, without conflating it with Ax=b.</summary>
    public static bool VerifyCoefficientRow(string? response, long[] row)
    {
        if (!Safe(response)) return false;
        var normalized = response!.Trim();
        // M09 fixture accepts an ordered tuple as well as a column vector.
        // Parentheses are only an alternate delimiter, not an expression parser.
        if (normalized.StartsWith('(') && normalized.EndsWith(')'))
            normalized = "[" + normalized[1..^1] + "]";
        if (!Vector(normalized, out var actual) ||
            actual.Length != row.Length) return false;
        for (var i = 0; i < row.Length; i++)
            if (actual[i] != Q.From(row[i])) return false;
        return true;
    }

    /// <summary>
    /// Closed 2x2 system classification only; a dependent-system parameterisation
    /// or an explanation of rank remains subject to M05 teacher review.
    /// Computes coefficient and augmented rank using exact integer minors.
    /// </summary>
    public static bool VerifySystemClassification(
        string? response, long[,] coefficients, long[] rhs)
    {
        if (!Safe(response) || coefficients.GetLength(0) != 2 ||
            coefficients.GetLength(1) != 2 || rhs.Length != 2)
            return false;
        BigInteger a = coefficients[0, 0], b = coefficients[0, 1];
        BigInteger c = coefficients[1, 0], d = coefficients[1, 1];
        BigInteger e = rhs[0], f = rhs[1];
        var det = a * d - b * c;
        // Rank(A)=0 needs its own check: every 2x2 augmented minor
        // vanishes even if 0x+0y=nonzero is contradictory.
        var allCoefficientsZero = a.IsZero && b.IsZero && c.IsZero && d.IsZero;
        var consistentSingular = allCoefficientsZero
            ? e.IsZero && f.IsZero
            : a * f == c * e && b * f == d * e;
        var expected = det != 0 ? "unique" :
            (consistentSingular ? "infinite" : "none");
        var actual = response!.Trim().ToLowerInvariant();
        actual = actual switch
        {
            "infinitely many" or "infinitely_many" or "dependent" => "infinite",
            "no solution" or "no solutions" or "inconsistent" => "none",
            "one solution" or "unique solution" => "unique",
            _ => actual
        };
        return actual == expected;
    }

    /// <summary>
    /// Exact Ax=b substitution check for a source row-reduction result.
    /// The reversible row operations and written proof are NOT graded here.
    /// </summary>
    public static bool VerifySystemSolution(
        string? response, long[,] coefficients, long[] rhs)
    {
        if (!Safe(response)) return false;
        var answer = response!.Trim();
        if (answer.StartsWith("x=", StringComparison.Ordinal))
            answer = answer[2..].Trim();
        if (!Vector(answer, out var x)) return false;
        var rows = coefficients.GetLength(0);
        if (rows < 2 || rows > 3 || coefficients.GetLength(1) != x.Length ||
            rhs.Length != rows) return false;
        for (var row = 0; row < rows; row++)
        {
            var total = Q.From(0);
            for (var col = 0; col < x.Length; col++)
                total += Q.From(coefficients[row, col]) * x[col];
            if (total != Q.From(rhs[row])) return false;
        }
        return true;
    }

    /// <summary>
    /// M07/M08/M10 typed, closed inverse-eligibility disposition. Distinguish
    /// singular, non-square and incompatible A^-1 b dimensions. These are
    /// course-owned source oracles, not shared engine registration; do not
    /// substitute this for the pending PR600 inverse_eligibility route.
    /// </summary>
    public static bool VerifyInverseEligibility(
        string? response, long[,] coefficients, int rhsLength = 0)
    {
        if (!Safe(response) || rhsLength < 0) return false;
        var rows = coefficients.GetLength(0);
        var columns = coefficients.GetLength(1);
        if (rows < 2 || rows > 3 || columns < 2 || columns > 3) return false;
        string expected;
        if (rows != columns)
            expected = "not_square";
        else if (rhsLength != 0 && rhsLength != rows)
            expected = "dimension_mismatch";
        else
        {
            BigInteger det;
            if (rows == 2)
                det = (BigInteger)coefficients[0, 0] * coefficients[1, 1] -
                    (BigInteger)coefficients[0, 1] * coefficients[1, 0];
            else
            {
                BigInteger a = coefficients[0, 0], b = coefficients[0, 1], c = coefficients[0, 2];
                BigInteger d = coefficients[1, 0], e = coefficients[1, 1], f = coefficients[1, 2];
                BigInteger g = coefficients[2, 0], h = coefficients[2, 1], i = coefficients[2, 2];
                det = a * (e * i - f * h) - b * (d * i - f * g) +
                    c * (d * h - e * g);
            }
            expected = det.IsZero ? "singular" : "invertible";
        }
        var actual = response!.Trim().ToLowerInvariant().Replace(' ', '_');
        actual = actual switch
        {
            "no_inverse" or "inverse_undefined_due_to_zero_determinant"
                or "det(a)=0;_inverse_undefined" or "singular;_no_inverse" => "singular",
            "invalid_dimensions" or "incompatible_dimensions"
                or "reject_dimension_mismatch" => "dimension_mismatch",
            "nonsquare" or "non_square" or "not_square;_a^-1_undefined"
                or "dimension-incompatible_for_inversion" => "not_square",
            _ => actual
        };
        return actual == expected;
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

    private static bool SafeTeacherExplanation(string? value) =>
        !string.IsNullOrWhiteSpace(value) &&
        value.Length <= MaxResponseLength &&
        !value.Contains('\0') &&
        !value.Contains('>') &&
        !value.Replace("<-", "", StringComparison.Ordinal).Contains('<');

    private static bool Safe(string? value) =>
        !string.IsNullOrWhiteSpace(value) &&
        value.Length <= MaxResponseLength &&
        !value.Contains('<') && !value.Contains('>') &&
        !value.Contains('\0') &&
        // Preserve whitespace around separators without merging two distinct
        // number tokens, e.g. source painting x=[1 2,2,6] must not mean 12.
        !Regex.IsMatch(value, @"(?<=\d)\s+(?=\d)");

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
            if (chunks.Length > 2) return false;

            // Original MVP 10.6H supplies a finite-decimal 3x3 inverse.
            // Interpret its entries as exact rationals, never binary floating
            // point approximations; reject exponents and unbounded precision.
            var numeratorText = chunks[0];
            var decimalPoint = numeratorText.IndexOf('.');
            BigInteger numerator;
            var scale = BigInteger.One;
            if (decimalPoint < 0)
            {
                if (!BigInteger.TryParse(numeratorText, NumberStyles.AllowLeadingSign,
                        CultureInfo.InvariantCulture, out numerator))
                    return false;
            }
            else
            {
                if (numeratorText.IndexOf('.', decimalPoint + 1) >= 0)
                    return false;
                var wholeText = numeratorText[..decimalPoint];
                var fractionText = numeratorText[(decimalPoint + 1)..];
                if (fractionText.Length is < 1 or > 12 ||
                    !fractionText.All(static digit => digit is >= '0' and <= '9'))
                    return false;
                var negative = wholeText.StartsWith("-", StringComparison.Ordinal);
                BigInteger whole = BigInteger.Zero;
                if (wholeText is not ("" or "+" or "-") &&
                    !BigInteger.TryParse(wholeText, NumberStyles.AllowLeadingSign,
                        CultureInfo.InvariantCulture, out whole))
                    return false;
                if (!BigInteger.TryParse(fractionText, NumberStyles.None,
                        CultureInfo.InvariantCulture, out var fraction))
                    return false;
                scale = BigInteger.Pow(10, fractionText.Length);
                numerator = whole * scale + (negative ? -fraction : fraction);
            }

            var denominator = scale;
            if (chunks.Length == 2)
            {
                if (!BigInteger.TryParse(chunks[1], NumberStyles.AllowLeadingSign,
                        CultureInfo.InvariantCulture, out var divisor) || divisor.IsZero)
                    return false;
                denominator *= divisor;
            }
            value = Canonical(numerator, denominator);
            return true;
        }
    }
}
