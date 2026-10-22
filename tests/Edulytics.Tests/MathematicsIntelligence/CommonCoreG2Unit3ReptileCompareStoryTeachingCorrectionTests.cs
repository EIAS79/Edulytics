using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit3ReptileCompareStoryTeachingCorrectionTests
{
    [Fact]
    public void OppositeKeywordSourceComparisonsAndEightReptilesRemainAccurate()
    {
        var p=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit3ReptileCompareStoryTeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        var en=p.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("62+19=81",en.WorkedExamples);
        Assert.Contains("71−17=54",en.WorkedExamples);
        Assert.Contains("28/98/46/65",en.QuickSummary);
        Assert.Contains("83/55/71/38",en.QuickSummary);
        Assert.Contains("not a repeat lesson on physically placing rulers",en.Explanation);
        Assert.Equal(CommonCoreG2Unit3ReptileCompareStoryTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(p.Doc,p.Lesson));
    }
    [Fact]
    public void UnknownPersistedVersionOrChangedOriginalBodyFailClosed()
    {
        var p=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit3ReptileCompareStoryTeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit3ReptileCompareStoryTeachingCorrection
            .CanUpgradeExisting(p.Doc,p.Lesson,"unknown"));
        var original=p.Lesson.CanonicalBodySha256;
        try {
            p.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit3ReptileCompareStoryTeachingCorrection.Apply(p.Doc));
        } finally { p.Lesson.CanonicalBodySha256=original; }
    }
}
