using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// A course-owned, first-party source math oracle for L01 reviewer fixtures.
/// This does NOT register a student Practice family, implement a shared grader,
/// or authorize selection. The collision-owned exact verifier is still pending.
/// Unlike four fixed fixtures, this oracle can enumerate bounded exact variants
/// and refuse inputs whose exact output has not been reviewed here.
/// </summary>
public static class UsCcssHsAdvTrigGeoU01L01BoundedExactSourceOracle
{
    public const string SourceFamily = UsCcssHsAdvTrigGeoU01L01PracticeCandidate.SourceFamily;
    public const string LessonCode = UsCcssHsAdvTrigGeoU01L01PracticeCandidate.LessonCode;
    public const string Readiness = "REVIEW_ORACLE_ONLY_NOT_ROUTED";
    public const string AngleMode = "radians";
    public const int TeacherProofRubricMaxPoints = 6;

    public static bool StudentPracticeRoutingAuthorized => false;
    public static bool CanAutomaticallyScoreProof => false;

    /// <summary>
    /// Codes follow the existing reviewer/kernel handoff: function 0=sin,
    /// 1=cos, 2=tan; transform 0=pi-theta, 1=pi+theta, 2=2pi-theta.
    /// Theta is twelfths of pi. Allowed [-24,24] correspond to [-2pi,2pi].
    /// Only multiples 2 or 3 are allowed: 30-degree and 45-degree axes.
    /// No floating-point operations or arbitrary-expression evaluation.
    /// </summary>
    public sealed record ExactReviewCase(
        int Twelfths, int FunctionCode, int TransformCode, int ResultTwelfths,
        string Prompt, string ExpectedStatus, string? CanonicalExact)
    {
        public bool IsScorable => ExpectedStatus == "OK" && CanonicalExact is not null;
    }

    public static bool TryBuild(
        string? lessonCode, string? angleMode, int twelfths,
        int functionCode, int transformCode, out ExactReviewCase? result)
    {
        result = null;
        if (!string.Equals(lessonCode, LessonCode, StringComparison.Ordinal)
            || !string.Equals(angleMode, AngleMode, StringComparison.Ordinal)
            || twelfths < -24 || twelfths > 24
            || (twelfths % 2 != 0 && twelfths % 3 != 0)
            || functionCode < 0 || functionCode > 2
            || transformCode < 0 || transformCode > 2)
            return false;

        var transformed = transformCode switch
        {
            0 => 12 - twelfths,
            1 => 12 + twelfths,
            _ => 24 - twelfths
        };
        // Euclidean remainder, including signed base angles.
        var position = ((transformed % 24) + 24) % 24;
        var referenceAngle = position <= 6 ? position
            : position <= 12 ? 12 - position
            : position <= 18 ? position - 12
            : 24 - position;
        if (!TryGetFirstQuadrantExact(referenceAngle,
                out var positiveSin, out var positiveCos, out var positiveTan))
            return false;

        var sinNegative = position > 12;
        var cosNegative = position > 6 && position < 18;
        string? exact;
        string status;
        if (functionCode == 2 && positiveCos == "0")
        {
            exact = null;
            status = "OUT_OF_DOMAIN";
        }
        else
        {
            exact = functionCode switch
            {
                0 => SignedExact(positiveSin, sinNegative),
                1 => SignedExact(positiveCos, cosNegative),
                _ => SignedExact(positiveTan!, sinNegative != cosNegative)
            };
            status = "OK";
        }

        var functionName = functionCode switch { 0 => "sin", 1 => "cos", _ => "tan" };
        var thetaText = FormatPiTwelfths(twelfths);
        var transformedText = transformCode switch
        {
            0 => $"pi - ({thetaText})",
            1 => $"pi + ({thetaText})",
            _ => $"2*pi - ({thetaText})"
        };
        var prompt = $"Evaluate {functionName}({transformedText}) exactly (radians).";
        result = new ExactReviewCase(twelfths, functionCode, transformCode,
            position, prompt, status, exact);
        return true;
    }

    /// <summary>
    /// Non-routed reviewer classification for a parameterized exact-angle case.
    /// Uses ONLY the current oracle's own computed expected value and the already
    /// approved finite aliases in the L01 source fixtures. No decimal tolerance,
    /// arbitrary expression execution, or teacher-proof scoring is permitted.
    /// A future shared selected Practice verifier must independently enforce this.
    /// </summary>
    public static string ClassifyReviewerResponse(
        string? lessonCode, string? angleMode, int twelfths,
        int functionCode, int transformCode, string? submission)
    {
        if (!string.Equals(angleMode, AngleMode, StringComparison.Ordinal))
            return "ANGLE_MODE_MISMATCH";
        if (!TryBuild(lessonCode, angleMode, twelfths, functionCode, transformCode,
                out var expected) || expected is null)
            return "LESSON_OR_CASE_MISMATCH";
        if (!expected.IsScorable)
            return "OUT_OF_DOMAIN";
        if (string.IsNullOrWhiteSpace(submission) || submission.Length > 80
            || Regex.IsMatch(submission, @"(?<=\d)\s+(?=\d)"))
            return "MALFORMED_OR_UNSUPPORTED";

        // Strip permitted formatting whitespace only AFTER rejecting a split
        // numeric token: 'sqrt(1 2)' must not become 'sqrt(12)'.
        var answer = string.Concat(submission.Where(c => !char.IsWhiteSpace(c)));
        var canonical = expected.CanonicalExact!;
        var approved = ReviewedAliases(canonical);
        if (string.Equals(answer, canonical, StringComparison.Ordinal)
            || approved.Contains(answer, StringComparer.Ordinal))
            return "EXACT_ACCEPTED";

        // Scientific notation and decimals are never exact-answer substitutes,
        // even if their finite double value is very close to the true radical.
        if ((answer.Contains('.') || answer.Contains('e') || answer.Contains('E'))
            && double.TryParse(answer, NumberStyles.Float,
                CultureInfo.InvariantCulture, out _))
            return "DECIMAL_REJECTED_EXACT_REQUIRED";

        // Recognize wrong signs without interpreting any arbitrary expression.
        var opposite = canonical.StartsWith("-", StringComparison.Ordinal)
            ? canonical[1..] : "-" + canonical;
        if (answer == opposite || approved.Any(a =>
                answer == (a.StartsWith("-", StringComparison.Ordinal)
                    ? a[1..] : "-" + a))
            || RecognizedCanonicalExactForms.Contains(answer)
            || UsCcssHsAdvTrigGeoU01L01PracticeCandidate.ReviewCases
                .Any(c => c.ApprovedExactEquivalents.Contains(
                    answer, StringComparer.Ordinal)))
            return "INCORRECT";

        return "MALFORMED_OR_UNSUPPORTED";
    }

    // Exact, finite first-quadrant outputs with every possible sign. An exact
    // response from the wrong generated case is INCORRECT, not a syntax error.
    private static readonly HashSet<string> RecognizedCanonicalExactForms =
        new(StringComparer.Ordinal)
        {
            "0", "1", "-1", "1/2", "-1/2", "sqrt(2)/2", "-sqrt(2)/2",
            "sqrt(3)/2", "-sqrt(3)/2", "sqrt(3)", "-sqrt(3)",
            "sqrt(3)/3", "-sqrt(3)/3"
        };

    private static IReadOnlyList<string> ReviewedAliases(string canonical)
        => UsCcssHsAdvTrigGeoU01L01PracticeCandidate.ReviewCases
            .Where(c => c.IsScorable
                && string.Equals(c.Exact, canonical, StringComparison.Ordinal))
            .SelectMany(c => c.ApprovedExactEquivalents)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

    private static bool TryGetFirstQuadrantExact(
        int twelfths, out string sin, out string cos, out string? tan)
    {
        // Finite, independently checkable exact first-quadrant table.
        // No unapproved pi/12 or pi/8 radicals silently enter the Practice domain.
        switch (twelfths)
        {
            case 0:
                sin = "0"; cos = "1"; tan = "0"; return true;
            case 2:
                sin = "1/2"; cos = "sqrt(3)/2"; tan = "sqrt(3)/3"; return true;
            case 3:
                sin = "sqrt(2)/2"; cos = "sqrt(2)/2"; tan = "1"; return true;
            case 4:
                sin = "sqrt(3)/2"; cos = "1/2"; tan = "sqrt(3)"; return true;
            case 6:
                sin = "1"; cos = "0"; tan = null; return true;
            default:
                sin = ""; cos = ""; tan = null; return false;
        }
    }

    private static string SignedExact(string positiveValue, bool negate)
        => !negate || positiveValue == "0" ? positiveValue : "-" + positiveValue;

    private static string FormatPiTwelfths(int twelfths)
    {
        if (twelfths == 0) return "0";
        var n = Math.Abs(twelfths);
        var d = 12;
        var gcd = GreatestCommonDivisor(n, d);
        n /= gcd; d /= gcd;
        var signedNumerator = twelfths < 0 ? -n : n;
        var numeratorText = signedNumerator switch
        {
            1 => "pi",
            -1 => "-pi",
            _ => $"{signedNumerator}*pi"
        };
        return d == 1 ? numeratorText : $"{numeratorText}/{d}";
    }

    private static int GreatestCommonDivisor(int a, int b)
    {
        while (b != 0)
        {
            var remainder = a % b;
            a = b; b = remainder;
        }
        return a;
    }
}
