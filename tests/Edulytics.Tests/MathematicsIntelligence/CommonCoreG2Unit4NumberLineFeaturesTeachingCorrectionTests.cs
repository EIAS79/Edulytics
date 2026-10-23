using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit4NumberLineFeaturesTeachingCorrectionTests
{
    [Fact]
    public void ChoralYarnAndRealPublisherImageEvidenceSurviveOriginalSourceGuard()
    {
        var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit4NumberLineFeaturesTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var en=target.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("CHORAL COUNT",en.Explanation);
        Assert.Contains("100",en.Explanation);
        Assert.Contains("Jada",en.Explanation);
        Assert.Contains("Andre",en.Explanation);
        Assert.Contains("Elena",en.Explanation);
        Assert.Contains("not embedded as accessible numeric data",en.Explanation);
        Assert.Contains("image-specific diagram revisions",en.StepByStepSolutions);
        Assert.Equal(CommonCoreG2Unit4NumberLineFeaturesTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(target.Doc,target.Lesson));
    }
    [Fact]
    public void UnknownPredecessorOrChangedOriginalBodyCannotBeApplied()
    {
        var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit4NumberLineFeaturesTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit4NumberLineFeaturesTeachingCorrection
            .CanUpgradeExisting(target.Doc,target.Lesson,"unknown-version"));
        var original=target.Lesson.CanonicalBodySha256;
        try
        {
            target.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit4NumberLineFeaturesTeachingCorrection.Apply(target.Doc));
        }
        finally{target.Lesson.CanonicalBodySha256=original;}
    }
}
