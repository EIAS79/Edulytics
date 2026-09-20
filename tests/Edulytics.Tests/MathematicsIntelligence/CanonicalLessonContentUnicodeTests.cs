using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CanonicalLessonContentUnicodeTests
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
                    Inspect(
                        lesson.LessonCode,
                        translation.CultureCode,
                        "Title",
                        translation.Title,
                        failures);
                    Inspect(
                        lesson.LessonCode,
                        translation.CultureCode,
                        "Explanation",
                        translation.Explanation,
                        failures);
                    Inspect(
                        lesson.LessonCode,
                        translation.CultureCode,
                        "KeyConceptsAndRules",
                        translation.KeyConceptsAndRules,
                        failures);
                    Inspect(
                        lesson.LessonCode,
                        translation.CultureCode,
                        "WorkedExamples",
                        translation.WorkedExamples,
                        failures);
                    Inspect(
                        lesson.LessonCode,
                        translation.CultureCode,
                        "StepByStepSolutions",
                        translation.StepByStepSolutions,
                        failures);
                    Inspect(
                        lesson.LessonCode,
                        translation.CultureCode,
                        "CommonMistakes",
                        translation.CommonMistakes,
                        failures);
                    Inspect(
                        lesson.LessonCode,
                        translation.CultureCode,
                        "QuickSummary",
                        translation.QuickSummary,
                        failures);
                }
            }
        }

        Assert.True(
            failures.Count == 0,
            "Malformed UTF-16 learner content: " +
            string.Join(" | ", failures.Take(100)) +
            (failures.Count > 100
                ? $" (+{failures.Count - 100} more)"
                : string.Empty));
    }

    private static void Inspect(
        string lessonCode,
        string culture,
        string field,
        string value,
        ICollection<string> failures)
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
                    $"{lessonCode}:{culture}:{field}:index={i}:" +
                    $"high-surrogate=U+{(int)current:X4}");
                continue;
            }

            if (char.IsLowSurrogate(current))
            {
                failures.Add(
                    $"{lessonCode}:{culture}:{field}:index={i}:" +
                    $"orphan-low-surrogate=U+{(int)current:X4}");
            }
        }
    }
}
