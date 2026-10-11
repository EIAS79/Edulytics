using Edulytics.Data.Seeding;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CommonCoreG4MultiplesSourceCorrectionTests
{
    private static (CanonicalLessonContentPackDocument Doc,
        CanonicalLessonContentPackLesson Lesson,
        CanonicalLessonContentPackTranslation English) GetEffectiveLesson()
    {
        var matches = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .SelectMany(document =>
                document.Lessons
                    .Where(lesson => lesson.LessonCode ==
                        CommonCoreG4MultiplesSourceCorrection.LessonCode)
                    .Select(lesson => (document, lesson)))
            .ToArray();

        var match = Assert.Single(matches);
        var english = Assert.Single(match.lesson.Translations,
            x => string.Equals(x.CultureCode, "en", StringComparison.Ordinal));

        return (match.document, match.lesson, english);
    }

    [Fact]
    public void CorrectsOnlySourceBackedGrade4Multiples_WithIndependentNumericChecks()
    {
        var (doc, lesson, english) = GetEffectiveLesson();

        Assert.Equal("CCSS:4.OA.B.4", Assert.Single(lesson.OutcomeCodes));
        Assert.Equal(CommonCoreG4MultiplesSourceCorrection.CorrectedContentVersion,
            CanonicalLessonContentMaterializer
                .GetEffectiveContentVersion(doc, lesson));

        Assert.Contains("3 × 4 = 12", english.StepByStepSolutions);
        Assert.Contains("3 × 9 = 27", english.StepByStepSolutions);
        Assert.Contains("3 × 10 = 30", english.StepByStepSolutions);
        Assert.Contains("4 × 7 = 28", english.WorkedExamples);
        Assert.DoesNotContain("Identify the figure", english.StepByStepSolutions);
        Assert.DoesNotContain("classifying a figure", english.CommonMistakes);
        Assert.DoesNotContain("&nbsp;", english.WorkedExamples);

        for (var length = 1; length <= 5; length++)
        {
            var area = 3 * length;
            Assert.Equal(0, area % 3);
            Assert.Contains($"3 × {length} = {area}", english.WorkedExamples);
        }

        Assert.Equal(28, 4 * 7);
        Assert.NotEqual(0, 28 % 3);
    }

    [Fact]
    public void Correction_IsIdempotent_AndRefusesChangedSourceIdentity()
    {
        var (doc, lesson, _) = GetEffectiveLesson();
        var before = CanonicalLessonContentMaterializer
            .ComputeLessonFingerprint(doc, lesson);

        CanonicalLessonContentMaterializer.Materialize(doc);

        Assert.Equal(before, CanonicalLessonContentMaterializer
            .ComputeLessonFingerprint(doc, lesson));

        var original = lesson.CanonicalBodySha256;
        lesson.CanonicalBodySha256 = new string('0', 64);
        try
        {
            Assert.Throws<InvalidOperationException>(
                () => CommonCoreG4MultiplesSourceCorrection.Apply(doc));
        }
        finally
        {
            lesson.CanonicalBodySha256 = original;
        }
    }

    [Fact]
    public void Correction_DoesNotChangeOtherGrade4LessonVersion()
    {
        var (doc, _, _) = GetEffectiveLesson();
        var another = Assert.Single(doc.Lessons,
            x => x.LessonCode == "PED:US-CCSS-MATH:G4:U01:L02");
        Assert.Equal(CommonCoreRichContentCorrections.CorrectionContentVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc, another));
        Assert.False(CommonCoreG4MultiplesSourceCorrection.IsTarget(doc, another));
    }

    [Fact]
    public void HistoricalDatabaseUpgrade_RequiresExactPredecessorAndRejectsUnexpectedProse()
    {
        var (doc, lesson, _) = GetEffectiveLesson();

        CommonCoreG4MultiplesSourceCorrection.ValidateExistingBodyBeforeUpgrade(
            doc, lesson,
            CommonCoreRichContentCorrections.CorrectionContentVersion,
            Array.Empty<CurriculumLessonContentTranslation>());

        Assert.Throws<InvalidOperationException>(() =>
            CommonCoreG4MultiplesSourceCorrection.ValidateExistingBodyBeforeUpgrade(
                doc, lesson, "unrecognized-historical-version",
                Array.Empty<CurriculumLessonContentTranslation>()));

        var tampered = new CurriculumLessonContentTranslation
        {
            CultureCode = "en",
            Title = "Unrecognized content from another source",
            Explanation = "No source verification"
        };

        Assert.Throws<InvalidOperationException>(() =>
            CommonCoreG4MultiplesSourceCorrection.ValidateExistingBodyBeforeUpgrade(
                doc, lesson,
                CommonCoreRichContentCorrections.CorrectionContentVersion,
                new[] { tampered }));
    }

}
