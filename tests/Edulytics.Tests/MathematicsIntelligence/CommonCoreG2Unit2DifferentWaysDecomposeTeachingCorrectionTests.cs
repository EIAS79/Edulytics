using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit2DifferentWaysDecomposeTeachingCorrectionTests
{
    [Fact]
    public void SourceKeepsLinClareAndreOrderAndNumbers()
    {
        var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit2DifferentWaysDecomposeTeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        var en=target.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("71−56=15",en.WorkedExamples);
        Assert.Contains("Clare",en.Explanation);
        Assert.Contains("Lin",en.Explanation);
        Assert.Contains("65−28=37",en.WorkedExamples);
        Assert.Contains("82−37=45",en.WorkedExamples);
        Assert.Equal(CommonCoreG2Unit2DifferentWaysDecomposeTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(target.Doc,target.Lesson));
    }
    [Fact]
    public void OriginalBodyShaOrUnreviewedVersionFailClosed()
    {
        var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit2DifferentWaysDecomposeTeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit2DifferentWaysDecomposeTeachingCorrection
            .CanUpgradeExisting(target.Doc,target.Lesson,"wrong-version"));
        var old=target.Lesson.CanonicalBodySha256;
        try {
            target.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit2DifferentWaysDecomposeTeachingCorrection.Apply(target.Doc));
        } finally {target.Lesson.CanonicalBodySha256=old;}
    }
}
