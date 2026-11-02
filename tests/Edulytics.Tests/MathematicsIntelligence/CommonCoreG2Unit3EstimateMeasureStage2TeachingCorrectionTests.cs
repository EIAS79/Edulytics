using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit3EstimateMeasureStage2TeachingCorrectionTests
{
    [Fact]
    public void OriginalStage2TeacherLessonPreservesCentimetersAndSourceNotebookExample()
    {
        var p=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit3EstimateMeasureStage2TeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        var en=p.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("37−20=17",en.WorkedExamples);
        Assert.Contains("31 cm",en.WorkedExamples);
        Assert.Contains("11 cm",en.WorkedExamples);
        Assert.Contains("CENTIMETERS",en.Explanation);
        Assert.Contains("Math Stories Stage5",en.Explanation);
        Assert.Equal(CommonCoreG2Unit3EstimateMeasureStage2TeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(p.Doc,p.Lesson));
    }
    [Fact]
    public void OriginalShaOrPersistedVersionDriftIsRejected()
    {
        var p=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit3EstimateMeasureStage2TeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit3EstimateMeasureStage2TeachingCorrection
            .CanUpgradeExisting(p.Doc,p.Lesson,"unapproved"));
        var old=p.Lesson.CanonicalBodySha256;
        try {
            p.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit3EstimateMeasureStage2TeachingCorrection.Apply(p.Doc));
        } finally {p.Lesson.CanonicalBodySha256=old;}
    }
}
