using System.Text.RegularExpressions;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Independently authored checker for the RECORDING-SHEET mathematical part of
/// IM 2021 G2 U01 L01 "Check It Off". The physical two-card draw, partner
/// reasoning, checked-sheet provenance and teacher approval remain human tasks.
/// A correct result here does not mean the original activity is completed.
/// </summary>
public static class UsCcssG2Unit1FirstLessonExpressionRecord
{
    private static readonly Regex ExpressionPattern = new(
        @"^\s*(10|[0-9])\s*([+\-−])\s*(10|[0-9])\s*$",
        RegexOptions.CultureInvariant | RegexOptions.Compiled);

    public sealed record Review(
        bool ExpressionValid,
        bool TargetMatched,
        int? ComputedValue,
        string? NormalizedExpression,
        string Reason);

    public static Review Check(int markedTarget, string? studentExpression)
    {
        if (markedTarget is < 0 or > 10)
            return new Review(false, false, null, null, "Target outside recording-sheet 0..10");

        var match = ExpressionPattern.Match(studentExpression ?? string.Empty);
        if (!match.Success)
            return new Review(false, false, null, null,
                "Expected exactly two 0..10 integers and one plus or minus sign");

        var left = int.Parse(match.Groups[1].Value,
            System.Globalization.CultureInfo.InvariantCulture);
        var right = int.Parse(match.Groups[3].Value,
            System.Globalization.CultureInfo.InvariantCulture);
        var symbol = match.Groups[2].Value == "+" ? "+" : "−";
        var value = symbol == "+" ? left + right : left - right;
        if (value is < 0 or > 10)
            return new Review(true, false, value,
                $"{left} {symbol} {right}", "Value not in this lesson's 0..10 recording range");

        var matches = value == markedTarget;
        return new Review(true, matches, value, $"{left} {symbol} {right}",
            matches ? "Mathematical expression matches checked value; teacher activity evidence still required"
                    : "Student expression does not equal checked target");
    }

    /// <summary>
    /// Several mathematically different expressions for the same target
    /// may all be valid. No single canonical expression is required.
    /// </summary>
    public static IReadOnlyList<Review> CheckAll(int target, IEnumerable<string> expressions) =>
        expressions.Select(expression => Check(target, expression)).ToArray();
}
