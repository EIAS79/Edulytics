using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CanonicalLessonContentMaterializerTests
{
    [Fact]
    public void Materializer_IsDeterministicAndIdempotentAcrossCatalogue()
    {
        var documents = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .OrderBy(x => x.PackCode, StringComparer.Ordinal)
            .ThenBy(x => x.VersionCode, StringComparer.Ordinal)
            .ToArray();

        var before = documents
            .SelectMany(document => document.Lessons.Select(lesson => new
            {
                lesson.LessonCode,
                Fingerprint =
                    CanonicalLessonContentMaterializer
                        .ComputeLessonFingerprint(
                            document,
                            lesson),
                Version =
                    CanonicalLessonContentMaterializer
                        .GetEffectiveContentVersion(
                            document,
                            lesson)
            }))
            .ToDictionary(
                x => x.LessonCode,
                x => (x.Fingerprint, x.Version),
                StringComparer.Ordinal);

        foreach (var document in documents)
            CanonicalLessonContentMaterializer.Materialize(document);

        var after = documents
            .SelectMany(document => document.Lessons.Select(lesson => new
            {
                lesson.LessonCode,
                Fingerprint =
                    CanonicalLessonContentMaterializer
                        .ComputeLessonFingerprint(
                            document,
                            lesson),
                Version =
                    CanonicalLessonContentMaterializer
                        .GetEffectiveContentVersion(
                            document,
                            lesson)
            }))
            .ToDictionary(
                x => x.LessonCode,
                x => (x.Fingerprint, x.Version),
                StringComparer.Ordinal);

        Assert.Equal(4453, before.Count);
        Assert.Equal(before, after);
    }

    [Fact]
    public void QuantifiedRelationships_UsesReviewedEffectiveLearnerBody()
    {
        const string lessonCode =
            "PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-1:BUILD";

        var pair = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .SelectMany(document =>
                document.Lessons.Select(lesson => (document, lesson)))
            .Single(x =>
                string.Equals(
                    x.lesson.LessonCode,
                    lessonCode,
                    StringComparison.Ordinal));

        var english = Assert.Single(
            pair.lesson.Translations,
            translation =>
                translation.CultureCode.StartsWith(
                    "en",
                    StringComparison.OrdinalIgnoreCase));

        Assert.Equal(
            "supporting-practice-remediation-v1",
            CanonicalLessonContentMaterializer
                .GetEffectiveContentVersion(
                    pair.document,
                    pair.lesson));

        Assert.Contains(
            "Quantifying a relationship means expressing how quantities are connected by exact operations",
            english.Explanation,
            StringComparison.OrdinalIgnoreCase);

        Assert.Contains(
            "more than",
            english.CommonMistakes,
            StringComparison.OrdinalIgnoreCase);

        Assert.DoesNotContain(
            "Always state what each number, unit, operation or geometric property represents before calculating",
            english.KeyConceptsAndRules,
            StringComparison.OrdinalIgnoreCase);

        Assert.DoesNotContain(
            "Read the problem and identify the quantities or properties",
            english.StepByStepSolutions,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void MaterializedCatalogue_ContainsOnlyWellFormedUnicode()
    {
        var failures = new List<string>();

        foreach (var document in MathematicsCanonicalLessonContentSeeder
                     .LoadEmbeddedDocuments())
        {
            CanonicalLessonContentMaterializer.Materialize(document);

            foreach (var lesson in document.Lessons)
            {
                foreach (var translation in lesson.Translations)
                {
                    CheckWellFormed(
                        lesson.LessonCode,
                        translation.CultureCode,
                        "Title",
                        translation.Title,
                        failures);
                    CheckWellFormed(
                        lesson.LessonCode,
                        translation.CultureCode,
                        "Explanation",
                        translation.Explanation,
                        failures);
                    CheckWellFormed(
                        lesson.LessonCode,
                        translation.CultureCode,
                        "KeyConceptsAndRules",
                        translation.KeyConceptsAndRules,
                        failures);
                    CheckWellFormed(
                        lesson.LessonCode,
                        translation.CultureCode,
                        "WorkedExamples",
                        translation.WorkedExamples,
                        failures);
                    CheckWellFormed(
                        lesson.LessonCode,
                        translation.CultureCode,
                        "StepByStepSolutions",
                        translation.StepByStepSolutions,
                        failures);
                    CheckWellFormed(
                        lesson.LessonCode,
                        translation.CultureCode,
                        "CommonMistakes",
                        translation.CommonMistakes,
                        failures);
                    CheckWellFormed(
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
            "Malformed Unicode in materialized learner content: " +
            string.Join(" | ", failures.Take(100)));
    }

    [Fact]
    public void ReviewedCorrectionTargetSet_IsResolvedByOneAuthority()
    {
        var targets = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .SelectMany(document =>
                document.Lessons
                    .Where(lesson =>
                        CanonicalLessonContentMaterializer
                            .IsReviewedCorrectionTarget(
                                document,
                                lesson))
                    .Select(lesson => lesson.LessonCode))
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        Assert.NotEmpty(targets);
        Assert.Contains(
            "PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-1:BUILD",
            targets);
        Assert.Contains(
            "PED:CAMBRIDGE-INTL-MATH:S6:6F-3:BUILD",
            targets);
    }
    private static void CheckWellFormed(
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
                    $"{lessonCode}:{culture}:{field}:index={i}:U+{(int)current:X4}");
                continue;
            }

            if (char.IsLowSurrogate(current))
            {
                failures.Add(
                    $"{lessonCode}:{culture}:{field}:index={i}:U+{(int)current:X4}");
            }
        }
    }

}
