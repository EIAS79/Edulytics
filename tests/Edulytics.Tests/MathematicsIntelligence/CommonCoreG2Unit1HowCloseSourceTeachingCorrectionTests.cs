using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Original IM 2021 Grade 2 lesson 4 Stage C teaching test AUTHORING only.
/// Execution and independent student-parity evidence belong to Stage D.
/// </summary>
public sealed class CommonCoreG2Unit1HowCloseSourceTeachingCorrectionTests
{
    [Fact]
    public void HowCloseTeachingMatchesFiveCardsThreeChooseTargetTwenty()
    {
        var row = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(document => document.Lessons
                .Where(lesson =>
                    CommonCoreG2Unit1HowCloseSourceTeachingCorrection.IsTarget(document, lesson))
                .Select(lesson => (Document: document, Lesson: lesson)))
            .Single();
        Assert.Equal("PED:US-CCSS-MATH:G2:U01:L04", row.Lesson.LessonCode);
        var english = row.Lesson.Translations.Single(t => t.CultureCode == "en");
        Assert.Contains("THREE", english.Explanation);
        Assert.Contains("absolute distances", english.KeyConceptsAndRules);
        Assert.Contains("17 − 9 = 8", english.KeyConceptsAndRules);
        Assert.Contains("equally close", english.StepByStepSolutions);
        Assert.Contains("three or four cards", english.CommonMistakes);
        Assert.DoesNotContain("sense to them", english.QuickSummary);
        Assert.Equal(CommonCoreG2Unit1HowCloseSourceTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                row.Document, row.Lesson));
    }

    [Fact]
    public void HowCloseSourceHashAndEditionMismatchFailClosed()
    {
        var row = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(document => document.Lessons
                .Where(lesson =>
                    CommonCoreG2Unit1HowCloseSourceTeachingCorrection.IsTarget(document, lesson))
                .Select(lesson => (Document: document, Lesson: lesson)))
            .Single();
        Assert.True(CommonCoreG2Unit1HowCloseSourceTeachingCorrection.CanUpgradeExisting(
            row.Document, row.Lesson, CommonCoreRichContentCorrections.CorrectionContentVersion));
        Assert.False(CommonCoreG2Unit1HowCloseSourceTeachingCorrection.CanUpgradeExisting(
            row.Document, row.Lesson, "other-version"));

        var original = row.Lesson.CanonicalBodySha256;
        try
        {
            row.Lesson.CanonicalBodySha256 = new string('0', 64);
            Assert.Throws<InvalidOperationException>(() =>
                CommonCoreG2Unit1HowCloseSourceTeachingCorrection.Apply(row.Document));
        }
        finally
        {
            row.Lesson.CanonicalBodySha256 = original;
        }

        var previous = row.Document.ContentVersion;
        try
        {
            row.Document.ContentVersion = "independent-lesson-fixture-v1";
            Assert.False(CommonCoreG2Unit1HowCloseSourceTeachingCorrection
                .IsTarget(row.Document, row.Lesson));
        }
        finally
        {
            row.Document.ContentVersion = previous;
        }
    }
}
