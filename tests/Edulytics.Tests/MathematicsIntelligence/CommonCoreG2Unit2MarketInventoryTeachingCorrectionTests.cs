using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit2MarketInventoryTeachingCorrectionTests
{
    [Fact]
    public void MarketOptionalSourceConstraintsAndReportArePreserved()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2MarketInventoryTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var t=pair.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("EXACTLY100",t.Explanation);
        Assert.Contains("THREE",t.Explanation);
        Assert.Contains("THREE times",t.Explanation);
        Assert.Contains("restock",t.WorkedExamples,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("ONE cube",t.QuickSummary);
        Assert.Equal(CommonCoreG2Unit2MarketInventoryTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(pair.Doc,pair.Lesson));
    }
    [Fact]
    public void ChangedPublisherShaOrUnreviewedPredecessorFailClosed()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2MarketInventoryTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit2MarketInventoryTeachingCorrection
            .CanUpgradeExisting(pair.Doc,pair.Lesson,"unapproved-version"));
        var old=pair.Lesson.CanonicalBodySha256;
        try {
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit2MarketInventoryTeachingCorrection.Apply(pair.Doc));
        }
        finally {pair.Lesson.CanonicalBodySha256=old;}
    }
}
