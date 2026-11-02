using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Exact digital portions of 2021 IM Grade 2 U01 L01:
/// selecting addition/subtraction from two cards, matching expression values,
/// finding a partner to make ten, and recognizing sums of ten.
/// Original partner play, free-authored expressions and Math Community discussion
/// still require a teacher; an integer answer does not grade those activities.
/// </summary>
internal static class UsCcssG2Unit1FirstLessonQuestions
{
    public const string TwoCardOperation = "usccss.g2.u01.l01.two_card_operation";
    public const string MatchExpression = "usccss.g2.u01.l01.match_expression_value";
    public const string MakeTen = "usccss.g2.u01.l01.make_ten_missing_addend";
    public const string RecognizeTen = "usccss.g2.u01.l01.recognize_ten_sum";

    public sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        family is TwoCardOperation or MatchExpression or MakeTen or RecognizeTen;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unknown Grade 2 U01 L01 question family");

        var p = new Dictionary<string, int>(StringComparer.Ordinal);
        string prompt;
        string explanation;

        if (family == TwoCardOperation)
        {
            // Permit the original teacher's 10-and-7 example: addition may
            // exceed ten but the selected subtraction remains within 0..10.
            // Right > 0 prevents addition/subtraction yielding the same result.
            var left = random.Next(2, 11);
            var right = random.Next(1, left + 1);
            var operation = left + right > 10 ? 1 : random.Next(2);
            var target = operation == 0 ? left + right : left - right;
            p["left"] = left;
            p["right"] = right;
            p["operation"] = operation;
            prompt = $"You drew the number cards {left} and {right}. " +
                $"Which operation gives {target}? Enter 1 for addition (+), " +
                "or 2 for subtraction (−).";
            explanation = "Consider both a sum and a difference from the two cards. " +
                $"Only one of {left} + {right} and {left} − {right} equals {target}.";
        }
        else if (family == MatchExpression)
        {
            var operation = random.Next(2);
            var left = random.Next(0, 11);
            var right = operation == 0 ? random.Next(0, 11 - left)
                : random.Next(0, left + 1);
            p["left"] = left;
            p["right"] = right;
            p["operation"] = operation;
            var sign = operation == 0 ? "+" : "−";
            prompt = $"Match the expression {left} {sign} {right} to its value. " +
                "Enter the value from 0 through 10.";
            explanation = operation == 0
                ? "Add the two parts, using counters, counting on, or a known fact."
                : "Take away the second part, using counters, counting back, or a known fact.";
        }
        else if (family == MakeTen)
        {
            var part = random.Next(0, 11);
            var representation = random.Next(3);
            p["part"] = part;
            p["representation"] = representation;
            var support = representation switch
            {
                0 => "Imagine placing counters on a ten-frame.",
                1 => "Use a number bond for ten.",
                _ => "Imagine two number cards whose sum is ten."
            };
            prompt = $"{support} Complete {part} + ? = 10. " +
                "Enter the missing addend.";
            explanation = $"Ten can be made from {part} and the missing part. " +
                "Count up to ten or check with a number bond.";
        }
        else
        {
            var left = random.Next(0, 11);
            var right = random.Next(0, 11 - left);
            p["left"] = left;
            p["right"] = right;
            prompt = $"You are checking addition expressions for a total of ten. " +
                $"Does {left} + {right} have a value of 10? Enter 1 for yes or 0 for no.";
            explanation = "Add the two numbers. A sum of exactly ten is a yes; " +
                "any smaller sum is a no.";
        }

        return new Problem(family, prompt, explanation,
            AssessmentItemType.Numeric, p);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Valid(family, p))
            throw new InvalidOperationException("Invalid Grade 2 U01 L01 parameters");

        var answer = family switch
        {
            TwoCardOperation => p["operation"] + 1,
            MatchExpression => p["operation"] == 0
                ? p["left"] + p["right"] : p["left"] - p["right"],
            MakeTen => 10 - p["part"],
            RecognizeTen => p["left"] + p["right"] == 10 ? 1 : 0,
            _ => throw new InvalidOperationException("Unsupported question family")
        };
        return answer.ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> parameters,
        string answer) =>
        Valid(family, parameters) &&
        string.Equals(answer, Solve(family, parameters), StringComparison.Ordinal);

    private static bool Valid(string? family, IReadOnlyDictionary<string, int> p)
    {
        if (!Supports(family)) return false;

        if (family == MakeTen)
            return p.Count == 2 &&
                p.TryGetValue("part", out var part) && part is >= 0 and <= 10 &&
                p.TryGetValue("representation", out var representation) &&
                representation is >= 0 and <= 2;

        if (!p.TryGetValue("left", out var left) ||
            !p.TryGetValue("right", out var right))
            return false;

        if (family == RecognizeTen)
            return p.Count == 2 && left is >= 0 and <= 10 &&
                right >= 0 && left + right <= 10;

        if (p.Count != 3 ||
            !p.TryGetValue("operation", out var operation) ||
            operation is not (0 or 1))
            return false;

        if (family == TwoCardOperation)
            return left is >= 2 and <= 10 &&
                right >= 1 && right <= left &&
                (operation == 1 || left + right <= 10);

        if (family == MatchExpression)
            return left is >= 0 and <= 10 && right >= 0 &&
                (operation == 0 ? left + right <= 10 : right <= left);

        return false;
    }
}
