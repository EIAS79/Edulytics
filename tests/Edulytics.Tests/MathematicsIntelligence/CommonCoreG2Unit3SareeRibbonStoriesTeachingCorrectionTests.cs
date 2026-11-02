using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit3SareeRibbonStoriesTeachingCorrectionTests
{
    [Fact]
    public void FourOriginalRibbonProblemsPreserveTheStartUnknownAddition()
    {
        var p=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit3SareeRibbonStoriesTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var t=p.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("44−18=26",t.WorkedExamples);
        Assert.Contains("58−27=31",t.WorkedExamples);
        Assert.Contains("64−28=36",t.WorkedExamples);
        Assert.Contains("25+38=63",t.WorkedExamples);
        Assert.Contains("START UNKNOWN",t.WorkedExamples);
        Assert.Equal(CommonCoreG2Unit3SareeRibbonStoriesTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(p.Doc,p.Lesson));
    }
    [Fact]
    public void UnknownPredecessorOrOriginalSourceMutationIsRejected()
    {
        var p=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit3SareeRibbonStoriesTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit3SareeRibbonStoriesTeachingCorrection
            .CanUpgradeExisting(p.Doc,p.Lesson,"unapproved"));
        var sha=p.Lesson.CanonicalBodySha256;
        try {
            p.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit3SareeRibbonStoriesTeachingCorrection.Apply(p.Doc));
        } finally {p.Lesson.CanonicalBodySha256=sha;}
    }
}
