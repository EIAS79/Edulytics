using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C teaching source tests (written, not executed).</summary>
public sealed class CommonCoreG2Unit1AddWithin50SourceTeachingCorrectionTests
{
    [Fact]
    public void SevenCardHowCloseAndPlaceValueAreSourceSpecific()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons
                .Where(lesson=>
                    CommonCoreG2Unit1AddWithin50SourceTeachingCorrection.IsTarget(doc,lesson))
                .Select(lesson=>(Document:doc,Lesson:lesson)))
            .Single();
        Assert.Equal("PED:US-CCSS-MATH:G2:U01:L05",row.Lesson.LessonCode);
        var en=row.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("seven digit cards",en.Explanation);
        Assert.Contains("FOUR",en.Explanation);
        Assert.Contains("target of 50",en.Explanation);
        Assert.Contains("27 + 15",en.KeyConceptsAndRules);
        Assert.Contains("DIFFERENT card positions",en.StepByStepSolutions);
        Assert.Contains("three cards",en.CommonMistakes);
        Assert.Equal(CommonCoreG2Unit1AddWithin50SourceTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                row.Document,row.Lesson));
    }

    [Fact]
    public void OnlyOriginalIdentityAndExactPriorVersionMayBeUpgraded()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons
                .Where(lesson=>
                    CommonCoreG2Unit1AddWithin50SourceTeachingCorrection.IsTarget(doc,lesson))
                .Select(lesson=>(Document:doc,Lesson:lesson)))
            .Single();
        Assert.True(CommonCoreG2Unit1AddWithin50SourceTeachingCorrection.CanUpgradeExisting(
            row.Document,row.Lesson,CommonCoreRichContentCorrections.CorrectionContentVersion));
        Assert.False(CommonCoreG2Unit1AddWithin50SourceTeachingCorrection.CanUpgradeExisting(
            row.Document,row.Lesson,"unreviewed-old-version"));
        var old=row.Lesson.CanonicalBodySha256;
        try
        {
            row.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit1AddWithin50SourceTeachingCorrection.Apply(row.Document));
        }
        finally {row.Lesson.CanonicalBodySha256=old;}
    }
}
