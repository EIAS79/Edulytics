using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit3FeetFishTapeTeachingCorrectionTests
{
    [Fact]
    public void SourceFourFishTapeTargetsAndNoFormalConversionRequirement()
    {
        var p=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit3FeetFishTapeTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var t=p.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("A largemouth bass2 feet",t.Explanation);
        Assert.Contains("B spiny dogfish shark4 feet",t.Explanation);
        Assert.Contains("C catfish3 feet",t.Explanation);
        Assert.Contains("D koi2 feet6 inches",t.Explanation);
        Assert.Contains("NOT EXPECTED TO CONVERT",t.Explanation);
        Assert.Contains("no actual measurement provided",t.CommonMistakes);
        Assert.Equal(CommonCoreG2Unit3FeetFishTapeTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(p.Doc,p.Lesson));
    }
    [Fact]
    public void SourceShaAndPersistedVersionAreFailClosed()
    {
        var p=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit3FeetFishTapeTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit3FeetFishTapeTeachingCorrection
            .CanUpgradeExisting(p.Doc,p.Lesson,"not-reviewed"));
        var old=p.Lesson.CanonicalBodySha256;
        try {
            p.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit3FeetFishTapeTeachingCorrection.Apply(p.Doc));
        } finally {p.Lesson.CanonicalBodySha256=old;}
    }
}
