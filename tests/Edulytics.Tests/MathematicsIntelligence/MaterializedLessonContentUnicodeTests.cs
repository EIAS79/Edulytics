using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class MaterializedLessonContentUnicodeTests
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
            "Invalid UTF-16 learner content: " +
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
                    $"{lessonCode}:{culture}:{field}:index={index}:" +
                    $"codeUnit=U+{(int)current:X4}:lone-high-surrogate");
                continue;
            }

            if (char.IsLowSurrogate(current))
            {
                failures.Add(
                    $"{lessonCode}:{culture}:{field}:index={index}:" +
                    $"codeUnit=U+{(int)current:X4}:lone-low-surrogate");
            }
        }
    }
}
