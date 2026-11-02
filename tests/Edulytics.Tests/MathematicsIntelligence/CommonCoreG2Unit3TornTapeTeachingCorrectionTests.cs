using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit3TornTapeTeachingCorrectionTests
{
    [Fact]
    public void AuthenticNotebookShortDeskAndUnseenLongSideAreSourcedCorrectly()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit3TornTapeTeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        var en=pair.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("10−2=8",en.WorkedExamples);
        Assert.Contains("34−16=18",en.WorkedExamples);
        Assert.Contains("DOES NOT reveal the long-side ending tick",en.Explanation);
        Assert.Contains("2.OA.B.2",en.KeyConceptsAndRules);
        Assert.Equal(CommonCoreG2Unit3TornTapeTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(pair.Doc,pair.Lesson));
    }
    [Fact]
    public void UnknownPreviousVersionOrMismatchedOriginalShaRejected()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit3TornTapeTeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit3TornTapeTeachingCorrection
            .CanUpgradeExisting(pair.Doc,pair.Lesson,"unreviewed"));
        var old=pair.Lesson.CanonicalBodySha256;
        try {
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit3TornTapeTeachingCorrection.Apply(pair.Doc));
        } finally {pair.Lesson.CanonicalBodySha256=old;}
    }
}
