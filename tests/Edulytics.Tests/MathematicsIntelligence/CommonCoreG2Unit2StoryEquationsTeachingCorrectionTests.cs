using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit2StoryEquationsTeachingCorrectionTests
{
    [Fact]
    public void FourOriginalUnknownPositionsMatchActualLocalTeacherSource()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2StoryEquationsTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var t=pair.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("?+36=64",t.WorkedExamples);
        Assert.Contains("32+?=57",t.WorkedExamples);
        Assert.Contains("22+48",t.WorkedExamples);
        Assert.Contains("53+?=92",t.WorkedExamples);
        Assert.Contains("more than one",t.Explanation,StringComparison.OrdinalIgnoreCase);
        Assert.Equal(CommonCoreG2Unit2StoryEquationsTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(pair.Doc,pair.Lesson));
    }
    [Fact]
    public void UnknownPersistedVersionAndSourceHashCannotSilentlyUpgrade()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2StoryEquationsTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit2StoryEquationsTeachingCorrection
            .CanUpgradeExisting(pair.Doc,pair.Lesson,"untrusted"));
        var old=pair.Lesson.CanonicalBodySha256;
        try {
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit2StoryEquationsTeachingCorrection.Apply(pair.Doc));
        } finally {pair.Lesson.CanonicalBodySha256=old;}
    }
}
