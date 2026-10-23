using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Strict G2 U01 L08 source-learner teaching assertions, Stage C only.
/// </summary>
public sealed class CommonCoreG2Unit1PictureGraphSourceTeachingCorrectionTests
{
    [Fact]
    public void GraphReadingUsesKeyAndTeachesUnanswerableQuestions()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons
                .Where(lesson=>
                    CommonCoreG2Unit1PictureGraphSourceTeachingCorrection
                        .IsTarget(doc,lesson))
                .Select(lesson=>(Document:doc,Lesson:lesson)))
            .Single();
        Assert.Equal("PED:US-CCSS-MATH:G2:U01:L08",row.Lesson.LessonCode);
        var en=row.Lesson.Translations.Single(t=>t.CultureCode=="en");
        Assert.Contains("one-symbol picture graphs",en.Explanation);
        Assert.Contains("cannot supply unrecorded information",en.Explanation);
        Assert.Contains("key",en.StepByStepSolutions,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("graph key",en.CommonMistakes);
        Assert.Contains("cannot determine",en.StepByStepSolutions);
        Assert.Contains("unrecorded adults",en.CommonMistakes);
        Assert.Equal(CommonCoreG2Unit1PictureGraphSourceTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                row.Document,row.Lesson));
    }
    [Fact]
    public void SourceIdentityAndPreviousVersionAreGuarded()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons
                .Where(lesson=>
                    CommonCoreG2Unit1PictureGraphSourceTeachingCorrection
                        .IsTarget(doc,lesson))
                .Select(lesson=>(Document:doc,Lesson:lesson)))
            .Single();
        Assert.True(CommonCoreG2Unit1PictureGraphSourceTeachingCorrection.CanUpgradeExisting(
            row.Document,row.Lesson,
            CommonCoreRichContentCorrections.CorrectionContentVersion));
        Assert.False(CommonCoreG2Unit1PictureGraphSourceTeachingCorrection.CanUpgradeExisting(
            row.Document,row.Lesson,"unknown-old-version"));
        var source=row.Lesson.CanonicalBodySha256;
        try
        {
            row.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit1PictureGraphSourceTeachingCorrection.Apply(row.Document));
        }
        finally {row.Lesson.CanonicalBodySha256=source;}
    }
}
