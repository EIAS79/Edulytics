using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class MaterializedContentUnicodeIntegrityTests
{
    [Fact]
    public void EveryMaterializedLearnerContentFieldHasValidUtf16()
    {
        var failures = new List<string>();

        foreach (var document in MathematicsCanonicalLessonContentSeeder
                     .LoadEmbeddedDocuments())
        {
            foreach (var lesson in document.Lessons)
            {
                foreach (var translation in lesson.Translations)
                {
                    Check(
                        failures,
                        lesson.LessonCode,
                        translation.CultureCode,
                        nameof(translation.Title),
                        translation.Title);
                    Check(
                        failures,
                        lesson.LessonCode,
                        translation.CultureCode,
                        nameof(translation.Explanation),
                        translation.Explanation);
                    Check(
                        failures,
                        lesson.LessonCode,
                        translation.CultureCode,
                        nameof(translation.KeyConceptsAndRules),
                        translation.KeyConceptsAndRules);
                    Check(
                        failures,
                        lesson.LessonCode,
                        translation.CultureCode,
                        nameof(translation.WorkedExamples),
                        translation.WorkedExamples);
                    Check(
                        failures,
                        lesson.LessonCode,
                        translation.CultureCode,
                        nameof(translation.StepByStepSolutions),
                        translation.StepByStepSolutions);
                    Check(
                        failures,
                        lesson.LessonCode,
                        translation.CultureCode,
                        nameof(translation.CommonMistakes),
                        translation.CommonMistakes);
                    Check(
                        failures,
                        lesson.LessonCode,
                        translation.CultureCode,
                        nameof(translation.QuickSummary),
                        translation.QuickSummary);
                }
            }
        }

        Assert.True(
            failures.Count == 0,
            "Malformed UTF-16 found in materialized learner content: " +
            string.Join(" | ", failures.Take(100)) +
            (failures.Count > 100
                ? $" (+{failures.Count - 100} more)"
                : string.Empty));
    }

    private static void Check(
        ICollection<string> failures,
        string lessonCode,
        string culture,
        string field,
        string value)
    {
        for (var i = 0; i < value.Length; i++)
        {
            var current = value[i];

            if (char.IsHighSurrogate(current))
            {
                if (i + 1 < value.Length &&
                    char.IsLowSurrogate(value[i + 1]))
                {
                    i++;
                    continue;
                }

                failures.Add(
                    $"{lessonCode}/{culture}/{field}: " +
                    $"unpaired high surrogate U+{(int)current:X4} " +
                    $"at UTF-16 index {i}; context={Context(value, i)}");
                continue;
            }

            if (char.IsLowSurrogate(current))
            {
                failures.Add(
                    $"{lessonCode}/{culture}/{field}: " +
                    $"unpaired low surrogate U+{(int)current:X4} " +
                    $"at UTF-16 index {i}; context={Context(value, i)}");
            }
        }
    }

    private static string Context(
        string value,
        int index)
    {
        var start = Math.Max(0, index - 24);
        var length = Math.Min(
            value.Length - start,
            49);

        var slice = value.Substring(start, length);

        return string.Concat(
            slice.Select(ch =>
                char.IsSurrogate(ch)
                    ? $"\\u{(int)ch:X4}"
                    : ch.ToString()));
    }
}
