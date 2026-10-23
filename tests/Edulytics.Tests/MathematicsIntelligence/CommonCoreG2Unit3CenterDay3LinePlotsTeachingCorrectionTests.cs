using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit3CenterDay3LinePlotsTeachingCorrectionTests
{
    [Fact]
    public void OptionalSourceStage1DoesNotInventMeasuredObjectNumbers()
    {
        var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit3CenterDay3LinePlotsTeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        var en=target.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("UP TO EIGHT",en.Explanation);
        Assert.Contains("TWO questions",en.Explanation);
        Assert.Contains("not supplied by the source",en.Explanation);
        Assert.Contains("60−11=49",en.WorkedExamples);
        Assert.Contains("Stage2",en.Explanation);
        Assert.Equal(CommonCoreG2Unit3CenterDay3LinePlotsTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(target.Doc,target.Lesson));
    }
    [Fact]
    public void OldSourceBodyOrUnknownContentVersionCannotUpgrade()
    {
        var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit3CenterDay3LinePlotsTeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit3CenterDay3LinePlotsTeachingCorrection
            .CanUpgradeExisting(target.Doc,target.Lesson,"not-reviewed"));
        var sha=target.Lesson.CanonicalBodySha256;
        try {
            target.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit3CenterDay3LinePlotsTeachingCorrection.Apply(target.Doc));
        } finally {target.Lesson.CanonicalBodySha256=sha;}
    }
}
