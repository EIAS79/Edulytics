using System.Text.RegularExpressions;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Services.LessonContent;

/// <summary>
/// B01-wide optional first-party counters for an arithmetic equality that
/// ACTUALLY occurs in the source-derived learner's authored worked examples.
/// A wrong or chained equality does not receive an invented representation.
/// This is an instructional enhancement candidate, not academic L4 approval.
/// </summary>
public static partial class UsCcssB01VerifiedExpressionVisuals
{
    [GeneratedRegex(
        @"(?<![\d+])(?<left>\d{1,2})\s*(?<operator>[+\-−])\s*(?<right>\d{1,2})\s*=\s*(?<total>\d{1,2})(?!\s*[+\-−\d])",
        RegexOptions.CultureInvariant)]
    private static partial Regex AtomicEqualityRegex();

    public static IReadOnlyList<RichLessonVisual> Build(
        string lessonCode, CanonicalLessonTranslationRecord body,
        LessonPracticeContract? contract = null)
    {
        // Preserve the already source-certified, special G1 U01 L01 visuals.
        if (lessonCode == "PED:US-CCSS-MATH:G1:U01:L01")
            return [];

        // B01 is all of Grades 1-3 and a source-fixed prefix of Grade 4;
        // original lesson bodies are never rewritten by this visual adapter.
        var match = Regex.Match(lessonCode,
            @"^PED:US-CCSS-MATH:G(?<grade>[1-4]):U(?<unit>\d{2}):L(?<lesson>\d{2})$",
            RegexOptions.CultureInvariant);
        if (!match.Success)
            return [];
        // B01 ends at G4 U04 L20. Never alter content of G4 lessons
        // belonging to B02 or other US CCSS batches.
        if (match.Groups["grade"].Value == "4" &&
            (int.Parse(match.Groups["unit"].Value) > 4 ||
             int.Parse(match.Groups["unit"].Value) == 4 &&
             int.Parse(match.Groups["lesson"].Value) > 20))
            return [];

        // Add a source-number-derived model only for a verified contract
        // whose subject is specifically whole-number place value / rounding.
        if (contract?.SkillId == "supporting.number.place_value_rounding")
        {
            var placeValue = TryPlaceValueVisual(body.WorkedExamples);
            if (placeValue is not null)
                return [placeValue];
        }

        // Fraction bars are permitted only for fraction-domain contracts,
        // and only when a proper fraction is explicitly in a worked example.
        if (contract?.SkillId.StartsWith("fractions.", StringComparison.Ordinal) == true)
        {
            var fractionBar = TryFractionBarVisual(body.WorkedExamples);
            if (fractionBar is not null)
                return [fractionBar];
        }

        foreach (Match m in AtomicEqualityRegex().Matches(body.WorkedExamples ?? string.Empty))
        {
            var a = int.Parse(m.Groups["left"].Value);
            var b = int.Parse(m.Groups["right"].Value);
            var total = int.Parse(m.Groups["total"].Value);
            var op = m.Groups["operator"].Value;
            var addition = op == "+";

            // A ten-frame-sized concrete model; avoid dozens of cells in
            // advanced topics. Zero, negatives and large numbers use existing
            // exact verified examples instead of fabricated counter diagrams.
            if (a < 1 || b < 1 || a > 10 || b > 10 || total < 0 || total > 20)
                continue;
            if (addition ? a + b != total : a - b != total)
                continue;
            if (!addition && a < b)
                continue;

            var title = addition
                ? $"Show {a} and {b} counters making {total}"
                : $"Show {a} counters and remove {b} to leave {total}";
            var description = addition
                ? $"The first group has {a} dots and the second has {b}. " +
                  $"Count every dot once to check {a} + {b} = {total}."
                : $"Start with {a} counters. Remove the last {b}; " +
                  $"{total} remain. Check {a} − {b} = {total}.";
            var rows = addition
                ? new List<RichLessonVisualRow>
                {
                    new() { Label = $"First addend: {a}", Cells = Dots(a) },
                    new() { Label = $"Second addend: {b}", Cells = Dots(b) }
                }
                : new List<RichLessonVisualRow>
                {
                    new() { Label = $"Start: {a}", Cells = Dots(a) },
                    new() {
                        Label = $"Remove: {b}", Cells = Enumerable.Repeat("○", b).ToList()
                    },
                    new() { Label = $"Remain: {total}", Cells = Dots(total) }
                };
            return
            [
                new RichLessonVisual {
                    Kind = RichLessonVisualKind.PlaceValueChart,
                    Title = title,
                    Description = description,
                    PrimaryLabel = "Source-derived, independently checked counter model",
                    Rows = rows
                }
            ];
        }
        return [];
    }

    private static RichLessonVisual? TryPlaceValueVisual(string? workedExamples)
    {
        if (string.IsNullOrWhiteSpace(workedExamples)) return null;
        var match = Regex.Match(workedExamples,
            @"(?<![\d,])(?:\d{1,3}(?:,\d{3})+|\d{2,7})(?![\d,])",
            RegexOptions.CultureInvariant);
        if (!match.Success) return null;
        var digits = match.Value.Replace(",", "", StringComparison.Ordinal);
        if (!int.TryParse(digits, out var number) || number < 10 ||
            number > 1000000) return null;

        var names = new[] { "Millions", "Hundred thousands", "Ten thousands",
            "Thousands", "Hundreds", "Tens", "Ones" };
        var places = new[] { 1000000, 100000, 10000, 1000, 100, 10, 1 };
        var padded = digits.PadLeft(7, '0');
        var rows = new List<RichLessonVisualRow>();
        for (var i = 0; i < padded.Length; i++)
        {
            var digit = padded[i] - '0';
            if (digit == 0 && rows.Count == 0) continue;
            rows.Add(new RichLessonVisualRow {
                Label = names[i],
                Cells = [digit.ToString(), (digit * places[i]).ToString()]
            });
        }
        return new RichLessonVisual {
            Kind = RichLessonVisualKind.PlaceValueChart,
            Title = $"Read {number:N0} by place value",
            Description = $"The source-derived example includes {number:N0}. " +
                "Each digit is shown with its positional value; " +
                "combine the place values to recover the original number.",
            PrimaryLabel = "Digit and value",
            Rows = rows
        };
    }

    private static RichLessonVisual? TryFractionBarVisual(string? workedExamples)
    {
        if (string.IsNullOrWhiteSpace(workedExamples)) return null;
        var match = Regex.Match(workedExamples,
            @"\\frac\s*\{\s*(?<n>\d{1,2})\s*\}\s*\{\s*(?<d>\d{1,2})\s*\}",
            RegexOptions.CultureInvariant);
        if (!match.Success) return null;
        var n = int.Parse(match.Groups["n"].Value);
        var d = int.Parse(match.Groups["d"].Value);
        if (n < 1 || d < 2 || d > 12 || n > d) return null;
        return new RichLessonVisual {
            Kind = RichLessonVisualKind.FractionBars,
            Title = $"Show {n}/{d} as equal parts",
            Description = $"Split one whole into {d} equal parts and shade {n}. " +
                $"The shaded portion is exactly {n}/{d}.",
            PrimaryLabel = "Equal fractional parts",
            Rows = [new RichLessonVisualRow {
                Label = $"Fraction {n}/{d}", Numerator = n, Denominator = d
            }]
        };
    }

    private static List<string> Dots(int count) =>
        Enumerable.Repeat("●", count).ToList();
}
