using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit3StandardUnitsTeachingCorrectionTests
{
    [Fact]
    public void TrueOriginalStringAndTwoStrawLengthsRemainDistinct()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit3StandardUnitsTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var t=pair.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("40-cm",t.WorkedExamples);
        Assert.Contains("10-cm",t.WorkedExamples);
        Assert.Contains("5-cm",t.WorkedExamples);
        Assert.Contains("40",t.WorkedExamples);
        Assert.Contains("no gaps",t.KeyConceptsAndRules);
        Assert.Equal(CommonCoreG2Unit3StandardUnitsTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(pair.Doc,pair.Lesson));
    }
    [Fact]
    public void OriginalSourceShaAndPredecessorPersistenceAreFailClosed()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit3StandardUnitsTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit3StandardUnitsTeachingCorrection
            .CanUpgradeExisting(pair.Doc,pair.Lesson,"not-reviewed"));
        var before=pair.Lesson.CanonicalBodySha256;
        try{
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit3StandardUnitsTeachingCorrection.Apply(pair.Doc));
        }finally{pair.Lesson.CanonicalBodySha256=before;}
    }
}
