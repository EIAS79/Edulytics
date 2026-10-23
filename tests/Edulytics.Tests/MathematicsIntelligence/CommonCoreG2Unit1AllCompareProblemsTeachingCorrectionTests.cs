using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Original 2021 content source guards, written not executed.</summary>
public sealed class CommonCoreG2Unit1AllCompareProblemsTeachingCorrectionTests
{
    [Fact]
    public void OriginalSixCompareProblemsReplaceGenericRichContent()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(lesson=>
                CommonCoreG2Unit1AllCompareProblemsTeachingCorrection.IsTarget(doc,lesson))
                .Select(lesson=>(Doc:doc,Lesson:lesson))).Single();
        Assert.Equal("PED:US-CCSS-MATH:G2:U01:L16",pair.Lesson.LessonCode);
        var en=pair.Lesson.Translations.Single(t=>t.CultureCode=="en");
        Assert.Contains("A Trip to the Library",en.Explanation);
        Assert.Contains("Three Reads",en.Explanation);
        Assert.Contains("29+8=37",en.KeyConceptsAndRules);
        Assert.Contains("47−20=27",en.StepByStepSolutions);
        Assert.Equal(CommonCoreG2Unit1AllCompareProblemsTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(pair.Doc,pair.Lesson));
    }
    [Fact]
    public void OriginalCanonicalBodyAndVersionMismatchFailClosed()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(lesson=>
                CommonCoreG2Unit1AllCompareProblemsTeachingCorrection.IsTarget(doc,lesson))
                .Select(lesson=>(Doc:doc,Lesson:lesson))).Single();
        Assert.True(CommonCoreG2Unit1AllCompareProblemsTeachingCorrection.CanUpgradeExisting(
            pair.Doc,pair.Lesson,CommonCoreRichContentCorrections.CorrectionContentVersion));
        Assert.False(CommonCoreG2Unit1AllCompareProblemsTeachingCorrection.CanUpgradeExisting(
            pair.Doc,pair.Lesson,"unknown-version"));
        var sha=pair.Lesson.CanonicalBodySha256;
        try
        {
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit1AllCompareProblemsTeachingCorrection.Apply(pair.Doc));
        }
        finally {pair.Lesson.CanonicalBodySha256=sha;}
    }
}
