using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CanonicalLessonContentUnicodeSafetyTests
{
    [Fact]
    public void EveryMaterializedLearnerContentFieldContainsOnlyValidUtf16()
    {
        var failures = new List<string>();

        foreach (var document in MathematicsCanonicalLessonContentSeeder
                     .LoadEmbeddedDocuments())
        {
            foreach (var lesson in document.Lessons)
            {
                foreach (var translation in lesson.Translations)
                {
                    Inspect(
                        failures,
                        lesson.LessonCode,
                        translation.CultureCode,
                        "Title",
                        translation.Title);
                    Inspect(
                        failures,
                        lesson.LessonCode,
                        translation.CultureCode,
                        "Explanation",
                        translation.Explanation);
                    Inspect(
                        failures,
                        lesson.LessonCode,
                        translation.CultureCode,
                        "KeyConceptsAndRules",
                        translation.KeyConceptsAndRules);
                    Inspect(
                        failures,
                        lesson.LessonCode,
                        translation.CultureCode,
                        "WorkedExamples",
                        translation.WorkedExamples);
                    Inspect(
                        failures,
                        lesson.LessonCode,
                        translation.CultureCode,
                        "StepByStepSolutions",
                        translation.StepByStepSolutions);
                    Inspect(
                        failures,
                        lesson.LessonCode,
                        translation.CultureCode,
                        "CommonMistakes",
                        translation.CommonMistakes);
                    Inspect(
                        failures,
                        lesson.LessonCode,
                        translation.CultureCode,
                        "QuickSummary",
                        translation.QuickSummary);
                }
            }
        }

        Assert.True(
            failures.Count == 0,
            "Invalid UTF-16 learner content: " +
            string.Join(" | ", failures.Take(100)) +
            (failures.Count > 100
                ? $" (+{failures.Count - 100} more)"
                : string.Empty));
    }

    private static void Inspect(
        ICollection<string> failures,
        string lessonCode,
        string culture,
        string field,
        string value)
    {
        for (var index = 0; index < value.Length; index++)
        {
            var current = value[index];

            if (char.IsHighSurrogate(current))
            {
                if (index + 1 < value.Length &&
                    char.IsLowSurrogate(value[index + 1]))
                {
                    index++;
                    continue;
                }

                failures.Add(
                    $"{lessonCode}/{culture}/{field}@{index}:U+{(int)current:X4}");
                continue;
            }

            if (char.IsLowSurrogate(current))
            {
                failures.Add(
                    $"{lessonCode}/{culture}/{field}@{index}:U+{(int)current:X4}");
            }
        }
    }
}
