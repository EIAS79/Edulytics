using Edulytics.Data.Seeding;

namespace Edulytics.Tests.Acceptance;

public sealed class CambridgeStage6LessonContentAlignmentTests
{
    [Fact]
    public void EmbeddedStage6Pack_AppliesExactSkillCorrections()
    {
        var document = Stage6Document();

        var twoUnknowns = document.Lessons.Single(
            x => string.Equals(
                x.LessonCode,
                CambridgePrimaryStage6LessonContentCorrections.TwoUnknownsLessonCode,
                StringComparison.Ordinal));
        var scaleReading = document.Lessons.Single(
            x => string.Equals(
                x.LessonCode,
                CambridgePrimaryStage6LessonContentCorrections.ScaleReadingLessonCode,
                StringComparison.Ordinal));
        var fractionComparison = document.Lessons.Single(
            x => string.Equals(
                x.LessonCode,
                CambridgePrimaryStage6LessonContentCorrections.FractionComparisonLessonCode,
                StringComparison.Ordinal));

        var twoUnknownsEnglish = English(twoUnknowns);
        Assert.Contains("x + y = 46", twoUnknownsEnglish.WorkedExamples, StringComparison.Ordinal);
        Assert.Contains("y − x = 8", twoUnknownsEnglish.WorkedExamples, StringComparison.Ordinal);
        Assert.Contains("both original relationships", twoUnknownsEnglish.StepByStepSolutions, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("48+27", twoUnknownsEnglish.WorkedExamples, StringComparison.Ordinal);

        var scaleEnglish = English(scaleReading);
        Assert.Contains("(end value − start value) ÷ number of intervals", scaleEnglish.Explanation, StringComparison.Ordinal);
        Assert.Contains("2, 4, 5 or 10", scaleEnglish.KeyConceptsAndRules, StringComparison.Ordinal);
        Assert.Contains("spaces between marks", scaleEnglish.KeyConceptsAndRules, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("powers of ten", scaleEnglish.Explanation, StringComparison.OrdinalIgnoreCase);

        var fractionEnglish = English(fractionComparison);
        Assert.Contains("2/3 = 8/12", fractionEnglish.WorkedExamples, StringComparison.Ordinal);
        Assert.Contains("3/4 = 9/12", fractionEnglish.WorkedExamples, StringComparison.Ordinal);
        Assert.Contains("3/5 = 24/40", fractionEnglish.WorkedExamples, StringComparison.Ordinal);
        Assert.Contains("5/8 = 25/40", fractionEnglish.WorkedExamples, StringComparison.Ordinal);
        Assert.Contains("1/2 = 4/8", fractionEnglish.WorkedExamples, StringComparison.Ordinal);
        Assert.Contains("fraction bars", fractionEnglish.WorkedExamples, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CorrectionVersion_IsRestrictedToThreeExactLessonCodes()
    {
        var document = Stage6Document();

        var targetCodes = new HashSet<string>(StringComparer.Ordinal)
        {
            CambridgePrimaryStage6LessonContentCorrections.TwoUnknownsLessonCode,
            CambridgePrimaryStage6LessonContentCorrections.ScaleReadingLessonCode,
            CambridgePrimaryStage6LessonContentCorrections.FractionComparisonLessonCode
        };

        var corrected =
            document.Lessons
                .Where(
                    lesson => string.Equals(
                        CambridgePrimaryStage6LessonContentCorrections
                            .GetExpectedContentVersion(document, lesson),
                        CambridgePrimaryStage6LessonContentCorrections
                            .CorrectionContentVersion,
                        StringComparison.Ordinal))
                .Select(x => x.LessonCode)
                .ToHashSet(StringComparer.Ordinal);

        Assert.True(targetCodes.SetEquals(corrected));

        foreach (var lesson in document.Lessons.Where(x => targetCodes.Contains(x.LessonCode)))
        {
            Assert.True(
                CambridgePrimaryStage6LessonContentCorrections.CanUpgradeExisting(
                    document,
                    lesson,
                    CambridgePrimaryStage6LessonContentCorrections.BaseContentVersion));

            Assert.False(
                CambridgePrimaryStage6LessonContentCorrections.CanUpgradeExisting(
                    document,
                    lesson,
                    "unexpected-version"));
        }

        var nonTarget =
            document.Lessons.First(
                x => !targetCodes.Contains(x.LessonCode));

        Assert.Equal(
            document.ContentVersion,
            CambridgePrimaryStage6LessonContentCorrections
                .GetExpectedContentVersion(document, nonTarget));
        Assert.False(
            CambridgePrimaryStage6LessonContentCorrections.CanUpgradeExisting(
                document,
                nonTarget,
                document.ContentVersion));
    }

    [Fact]
    public void CorrectedLessons_RemainSupportingAndDoNotInventOutcomeMappings()
    {
        var document = Stage6Document();

        var targetCodes = new HashSet<string>(StringComparer.Ordinal)
        {
            CambridgePrimaryStage6LessonContentCorrections.TwoUnknownsLessonCode,
            CambridgePrimaryStage6LessonContentCorrections.ScaleReadingLessonCode,
            CambridgePrimaryStage6LessonContentCorrections.FractionComparisonLessonCode
        };

        var targets =
            document.Lessons
                .Where(x => targetCodes.Contains(x.LessonCode))
                .ToArray();

        Assert.Equal(3, targets.Length);
        Assert.All(targets, lesson => Assert.True(lesson.IsSupporting));
        Assert.All(targets, lesson => Assert.Empty(lesson.OutcomeCodes));
    }

    private static Edulytics.Core.Curriculum.CanonicalLessonContentPackDocument Stage6Document() =>
        MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .Single(
                x =>
                    string.Equals(
                        x.PackCode,
                        CambridgePrimaryStage6LessonContentCorrections.PackCode,
                        StringComparison.Ordinal) &&
                    string.Equals(
                        x.ContentVersion,
                        CambridgePrimaryStage6LessonContentCorrections.BaseContentVersion,
                        StringComparison.Ordinal));

    private static Edulytics.Core.Curriculum.CanonicalLessonContentPackTranslation English(
        Edulytics.Core.Curriculum.CanonicalLessonContentPackLesson lesson) =>
        lesson.Translations.Single(
            x => string.Equals(
                x.CultureCode,
                "en",
                StringComparison.Ordinal));
}
