using System.Text.RegularExpressions;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;

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
        string lessonCode, CanonicalLessonTranslationRecord body)
    {
        // Preserve the already source-certified, special G1 U01 L01 visuals.
        if (lessonCode == "PED:US-CCSS-MATH:G1:U01:L01")
            return [];

        // B01 is all of Grades 1-3 and a source-fixed prefix of Grade 4;
        // original lesson bodies are never rewritten by this visual adapter.
        if (!Regex.IsMatch(lessonCode,
                @"^PED:US-CCSS-MATH:G[1-4]:U\d{2}:L\d{2}$",
                RegexOptions.CultureInvariant))
            return [];

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

    private static List<string> Dots(int count) =>
        Enumerable.Repeat("●", count).ToList();
}
