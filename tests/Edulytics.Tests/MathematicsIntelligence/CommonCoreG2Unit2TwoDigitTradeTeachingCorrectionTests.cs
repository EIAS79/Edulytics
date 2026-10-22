using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit2TwoDigitTradeTeachingCorrectionTests
{
    [Fact]
    public void TrueSourceExamplesAndUninventedCardsAreRetained()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit2TwoDigitTradeTeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        var t=pair.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("46−28=18",t.WorkedExamples);
        Assert.Contains("93−54=39",t.WorkedExamples);
        Assert.Contains("Diego",t.Explanation);
        Assert.Contains("do not invent",t.Explanation);
        Assert.Equal(CommonCoreG2Unit2TwoDigitTradeTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(pair.Doc,pair.Lesson));
    }
    [Fact]
    public void UnapprovedPersistenceVersionAndChangedSourceFailClosed()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit2TwoDigitTradeTeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit2TwoDigitTradeTeachingCorrection
            .CanUpgradeExisting(pair.Doc,pair.Lesson,"unapproved"));
        var old=pair.Lesson.CanonicalBodySha256;
        try {
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit2TwoDigitTradeTeachingCorrection.Apply(pair.Doc));
        }
        finally {pair.Lesson.CanonicalBodySha256=old;}
    }
}
