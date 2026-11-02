using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit3HandSpanLinePlotTeachingCorrectionTests
{
    [Fact]
    public void OriginalLinePlotHasClassVsFictionalSeparationAndCorrectXAxisMeaning()
    {
        var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit3HandSpanLinePlotTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var t=target.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("ONE measurement",t.Explanation);
        Assert.Contains("thumb",t.Explanation);
        Assert.Contains("FICTIONAL",t.Explanation);
        Assert.Contains("0 to 10 inches",t.Explanation);
        Assert.Equal(CommonCoreG2Unit3HandSpanLinePlotTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(target.Doc,target.Lesson));
    }
    [Fact]
    public void SourceIdentityOrUnapprovedPersistedVersionCannotUpgrade()
    {
        var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit3HandSpanLinePlotTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit3HandSpanLinePlotTeachingCorrection
            .CanUpgradeExisting(target.Doc,target.Lesson,"wrong-version"));
        var old=target.Lesson.CanonicalBodySha256;
        try {target.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit3HandSpanLinePlotTeachingCorrection.Apply(target.Doc));
        } finally {target.Lesson.CanonicalBodySha256=old;}
    }
}
