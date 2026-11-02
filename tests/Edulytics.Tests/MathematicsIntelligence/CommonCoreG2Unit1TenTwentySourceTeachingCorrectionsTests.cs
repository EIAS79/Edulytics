using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Stage C authoring: source-locked teaching, prior-body and edition safety.
/// These tests are intentionally NOT executed before Stage D.
/// </summary>
public sealed class CommonCoreG2Unit1TenTwentySourceTeachingCorrectionsTests
{
    private static readonly string[] TargetCodes =
    [
        "PED:US-CCSS-MATH:G2:U01:L02",
        "PED:US-CCSS-MATH:G2:U01:L03"
    ];

    [Fact]
    public void BothOriginalSourceLessonsHaveSpecificTenAndTwentyCubePedagogy()
    {
        var all = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var rows = all.SelectMany(doc => doc.Lessons
            .Where(lesson => CommonCoreG2Unit1TenTwentySourceTeachingCorrections
                .IsTarget(doc, lesson))
            .Select(lesson => new { Document = doc, Lesson = lesson }))
            .ToArray();
        Assert.Equal(TargetCodes, rows.Select(row => row.Lesson.LessonCode).ToArray());

        var l02 = rows[0].Lesson.Translations.Single(t => t.CultureCode == "en");
        var l03 = rows[1].Lesson.Translations.Single(t => t.CultureCode == "en");
        Assert.Contains("10 connecting cubes", l02.KeyConceptsAndRules);
        Assert.Contains("6 + ? = 10", l02.StepByStepSolutions);
        Assert.Contains("10 − 6 = 4", l02.StepByStepSolutions);
        Assert.DoesNotContain("Find the number that makes equations within 20 true",
            l02.KeyConceptsAndRules);

        Assert.Contains("two complete towers of ten", l03.KeyConceptsAndRules);
        Assert.Contains("20 − ? = 4", l03.StepByStepSolutions);
        Assert.Contains("25 − 5 = 20", l03.StepByStepSolutions);
        Assert.Contains("optional challenge", l03.CommonMistakes);
        Assert.All(rows, row => Assert.Equal(
            CommonCoreG2Unit1TenTwentySourceTeachingCorrections.CorrectedContentVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                row.Document, row.Lesson)));
    }

    [Fact]
    public void SourceIdentityChangesFailClosedBeforeOverwritingTeaching()
    {
        var original = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc => doc.Lessons
                .Where(lesson => CommonCoreG2Unit1TenTwentySourceTeachingCorrections
                    .IsTarget(doc, lesson))
                .Select(lesson => (Document: doc, Lesson: lesson)))
            .First();

        var prior = original.Lesson.CanonicalBodySha256;
        try
        {
            original.Lesson.CanonicalBodySha256 = new string('0', 64);
            Assert.Throws<InvalidOperationException>(() =>
                CommonCoreG2Unit1TenTwentySourceTeachingCorrections
                    .Apply(original.Document));
        }
        finally
        {
            original.Lesson.CanonicalBodySha256 = prior;
        }
    }

    [Fact]
    public void DifferentPackEditionDoesNotInheritCorrection()
    {
        var row = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc => doc.Lessons
                .Where(lesson => CommonCoreG2Unit1TenTwentySourceTeachingCorrections
                    .IsTarget(doc, lesson))
                .Select(lesson => (Document: doc, Lesson: lesson)))
            .First();
        var previous = row.Document.ContentVersion;
        try
        {
            row.Document.ContentVersion = "reviewed-independent-content-v1";
            Assert.False(CommonCoreG2Unit1TenTwentySourceTeachingCorrections
                .IsTarget(row.Document, row.Lesson));
        }
        finally
        {
            row.Document.ContentVersion = previous;
        }
    }

    [Fact]
    public void OnlyExactPriorVersionMayUpgrade()
    {
        var row = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc => doc.Lessons
                .Where(lesson => CommonCoreG2Unit1TenTwentySourceTeachingCorrections
                    .IsTarget(doc, lesson))
                .Select(lesson => (Document: doc, Lesson: lesson)))
            .First();

        Assert.True(CommonCoreG2Unit1TenTwentySourceTeachingCorrections
            .CanUpgradeExisting(row.Document, row.Lesson,
                CommonCoreRichContentCorrections.CorrectionContentVersion));
        Assert.False(CommonCoreG2Unit1TenTwentySourceTeachingCorrections
            .CanUpgradeExisting(row.Document, row.Lesson, "unknown-version"));
    }
}
