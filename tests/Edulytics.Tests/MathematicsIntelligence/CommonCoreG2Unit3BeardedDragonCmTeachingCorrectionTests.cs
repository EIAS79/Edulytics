using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit3BeardedDragonCmTeachingCorrectionTests
{
    [Fact]
    public void SourceBeardedDragonTwentyFourCmAndUnpublishedReptileMeasurementsArePreserved()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit3BeardedDragonCmTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var t=pair.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("24",t.Explanation);
        Assert.Contains("two complete10cm",t.Explanation,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("4 more",t.Explanation,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("NOT present",t.Explanation);
        Assert.Equal(CommonCoreG2Unit3BeardedDragonCmTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(pair.Doc,pair.Lesson));
    }
    [Fact]
    public void MismatchedOriginalShaOrPersistedPredecessorRejected()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit3BeardedDragonCmTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit3BeardedDragonCmTeachingCorrection
            .CanUpgradeExisting(pair.Doc,pair.Lesson,"unapproved"));
        var sha=pair.Lesson.CanonicalBodySha256;
        try {
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit3BeardedDragonCmTeachingCorrection.Apply(pair.Doc));
        }
        finally {pair.Lesson.CanonicalBodySha256=sha;}
    }
}
