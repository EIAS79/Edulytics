using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CommonCoreG2SourceGroundedFoundationsCorrectionsTests
{
    private static IReadOnlyList<(
        CanonicalLessonContentPackDocument Document,
        CanonicalLessonContentPackLesson Lesson,
        CanonicalLessonContentPackTranslation English)> GetTargets() =>
        MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(document => document.Lessons
                .Where(lesson =>
                    CommonCoreG2SourceGroundedFoundationsCorrections
                        .IsTarget(document, lesson))
                .Select(lesson => (
                    Document: document,
                    Lesson: lesson,
                    English: lesson.Translations.Single(x => x.CultureCode == "en"))))
            .ToArray();

    [Fact]
    public void AllFiveSourceVerifiedLessons_HaveOriginalGradeTwoInstruction()
    {
        var rows = GetTargets();
        Assert.Equal(5, rows.Count);
        Assert.Equal(5, rows.Select(x => x.Lesson.LessonCode)
            .Distinct(StringComparer.Ordinal).Count());

        foreach (var row in rows)
        {
            Assert.Equal(
                CommonCoreG2SourceGroundedFoundationsCorrections
                    .CorrectedContentVersion,
                CanonicalLessonContentMaterializer
                    .GetEffectiveContentVersion(row.Document, row.Lesson));
            Assert.True(row.English.WorkedExamples.Length > 150);
            Assert.Contains("Step 5:", row.English.StepByStepSolutions);
            Assert.DoesNotContain("Equivalent algebraic transformations",
                row.English.KeyConceptsAndRules);
            Assert.DoesNotContain("Read the hour and minute hands",
                row.English.KeyConceptsAndRules);
            Assert.DoesNotContain("Identify the figure and the attributes",
                row.English.StepByStepSolutions);
        }

        var addition = Assert.Single(rows, x =>
            x.Lesson.LessonCode.EndsWith("U01:L01", StringComparison.Ordinal));
        Assert.Equal("CCSS:2.OA.B.2", Assert.Single(addition.Lesson.OutcomeCodes));
        Assert.Contains("9 − 3 = 6", addition.English.WorkedExamples);
        Assert.Contains("7 + 3 = 10", addition.English.KeyConceptsAndRules);

        var fractions = rows.Where(x =>
            x.Lesson.LessonCode.Contains(":U06:", StringComparison.Ordinal))
            .ToArray();
        Assert.Equal(3, fractions.Length);
        Assert.All(fractions, x =>
            Assert.Contains("equal", x.English.KeyConceptsAndRules,
                StringComparison.OrdinalIgnoreCase));
        var halves = Assert.Single(fractions, x =>
            x.Lesson.LessonCode.EndsWith("L07", StringComparison.Ordinal));
        Assert.Contains("12-square-unit rectangle", halves.English.WorkedExamples);
        Assert.Equal(12, 2 * 6);
        Assert.Equal(12, 3 * 4);
        Assert.Equal(12, 4 * 3);

        var nonCongruent = Assert.Single(fractions, x =>
            x.Lesson.LessonCode.EndsWith("L08", StringComparison.Ordinal));
        Assert.Contains("equal AREA", nonCongruent.English.CommonMistakes);
        Assert.Contains("40, 50, 60, 70, 80",
            nonCongruent.English.WorkedExamples);
        Assert.Equal(new[] {"CCSS:2.G.A.3", "CCSS:2.NBT.A.2"},
            nonCongruent.Lesson.OutcomeCodes);

        var whole = Assert.Single(fractions, x =>
            x.Lesson.LessonCode.EndsWith("L09", StringComparison.Ordinal));
        Assert.Contains("3 thirds", whole.English.WorkedExamples);
        Assert.Contains("4 equal fourths", whole.English.KeyConceptsAndRules);

        var value = Assert.Single(rows, x =>
            x.Lesson.LessonCode.EndsWith("U09:L05", StringComparison.Ordinal));
        Assert.Equal("CCSS:2.NBT.A.1", Assert.Single(value.Lesson.OutcomeCodes));
        Assert.Contains("200 + 150 + 6 = 356", value.English.WorkedExamples);
        Assert.Contains("2 hundreds + 10 tens + 8 ones",
            value.English.WorkedExamples);
        Assert.Equal(356, 200 + 150 + 6);
        Assert.Equal(356, 350 + 6);
        Assert.Equal(308, 200 + 100 + 8);
    }

    [Fact]
    public void CorrectionsAreIdempotent_AndKeepNonTargetLessonsUnchanged()
    {
        var rows = GetTargets();
        foreach (var row in rows)
        {
            var before = CanonicalLessonContentMaterializer
                .ComputeLessonFingerprint(row.Document, row.Lesson);
            CanonicalLessonContentMaterializer.Materialize(row.Document);
            var after = CanonicalLessonContentMaterializer
                .ComputeLessonFingerprint(row.Document, row.Lesson);
            Assert.Equal(before, after);
        }

        var gradeTwo = rows[0].Document;
        var other = Assert.Single(gradeTwo.Lessons,
            x => x.LessonCode == "PED:US-CCSS-MATH:G2:U01:L02");
        Assert.False(CommonCoreG2SourceGroundedFoundationsCorrections
            .IsTarget(gradeTwo, other));
        Assert.NotEqual(
            CommonCoreG2SourceGroundedFoundationsCorrections.CorrectedContentVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(gradeTwo, other));
    }

    [Fact]
    public void SourceTamperingAndUnrecognizedOldDatabaseContentFailClosed()
    {
        var row = GetTargets()[0];
        var prevSha = row.Lesson.CanonicalBodySha256;
        row.Lesson.CanonicalBodySha256 = new string('0', 64);
        try
        {
            Assert.Throws<InvalidOperationException>(() =>
                CommonCoreG2SourceGroundedFoundationsCorrections
                    .Apply(row.Document));
        }
        finally
        {
            row.Lesson.CanonicalBodySha256 = prevSha;
        }

        Assert.True(CommonCoreG2SourceGroundedFoundationsCorrections
            .CanUpgradeExisting(row.Document, row.Lesson,
                CommonCoreRichContentCorrections.CorrectionContentVersion));
        Assert.False(CommonCoreG2SourceGroundedFoundationsCorrections
            .CanUpgradeExisting(row.Document, row.Lesson, "other"));

        CommonCoreG2SourceGroundedFoundationsCorrections
            .ValidateExistingBodyBeforeUpgrade(
                row.Document, row.Lesson,
                CommonCoreRichContentCorrections.CorrectionContentVersion,
                Array.Empty<CurriculumLessonContentTranslation>());

        Assert.Throws<InvalidOperationException>(() =>
            CommonCoreG2SourceGroundedFoundationsCorrections
                .ValidateExistingBodyBeforeUpgrade(
                    row.Document, row.Lesson, "other",
                    Array.Empty<CurriculumLessonContentTranslation>()));

        Assert.Throws<InvalidOperationException>(() =>
            CommonCoreG2SourceGroundedFoundationsCorrections
                .ValidateExistingBodyBeforeUpgrade(
                    row.Document, row.Lesson,
                    CommonCoreRichContentCorrections.CorrectionContentVersion,
                    new[] {new CurriculumLessonContentTranslation {
                        CultureCode = "en", Title = "Unexpected body",
                        Explanation = "Not original or corrected"
                    }}));
    }
}
