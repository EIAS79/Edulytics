using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit3BuildRulerTeachingCorrectionTests
{
    [Fact]
    public void SourceRulerZeroThrough25AndTrueAtoFTeacherOnlyArePreserved()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit3BuildRulerTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var x=pair.Lesson.Translations.Single(t=>t.CultureCode=="en");
        Assert.Contains("0,1,2",x.Explanation);
        Assert.Contains("TICK MARKS",x.Explanation);
        Assert.Contains("A–F",x.Explanation);
        Assert.Contains("NOT authoritative numeric lengths",x.Explanation);
        Assert.Contains("63−24=39",x.WorkedExamples);
        Assert.Equal(CommonCoreG2Unit3BuildRulerTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(pair.Doc,pair.Lesson));
    }
    [Fact]
    public void OldContentAndOriginalSourceMustRemainExpected()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit3BuildRulerTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit3BuildRulerTeachingCorrection
            .CanUpgradeExisting(pair.Doc,pair.Lesson,"unreviewed"));
        var sha=pair.Lesson.CanonicalBodySha256;
        try {
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit3BuildRulerTeachingCorrection.Apply(pair.Doc));
        } finally {pair.Lesson.CanonicalBodySha256=sha;}
    }
}
