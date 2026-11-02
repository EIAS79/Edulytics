using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit3MetersReptileTapeTeachingCorrectionTests
{
    [Fact]
    public void SevenExactSourcePreparationTapeModelsNotInvented()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit3MetersReptileTapeTeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        var t=pair.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("gila monster58cm",t.Explanation);
        Assert.Contains("baby alligator72cm",t.Explanation);
        Assert.Contains("baby cobra44cm",t.Explanation);
        Assert.Contains("komodo dragon180cm",t.Explanation);
        Assert.Contains("adult alligator3m36cm=336cm",t.Explanation);
        Assert.Contains("adult cobra1m90cm=190cm",t.Explanation);
        Assert.Contains("ribbon snake2m82cm=282cm",t.Explanation);
        Assert.Equal(CommonCoreG2Unit3MetersReptileTapeTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(pair.Doc,pair.Lesson));
    }
    [Fact]
    public void AlteredSourceShaAndPriorUnapprovedVersionFailClosed()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit3MetersReptileTapeTeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit3MetersReptileTapeTeachingCorrection
            .CanUpgradeExisting(pair.Doc,pair.Lesson,"not-reviewed"));
        var sha=pair.Lesson.CanonicalBodySha256;
        try {
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit3MetersReptileTapeTeachingCorrection.Apply(pair.Doc));
        } finally {pair.Lesson.CanonicalBodySha256=sha;}
    }
}
