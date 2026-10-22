using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit3EstimateMeasureCmTeachingCorrectionTests
{
    [Fact]
    public void OriginalFourRulerImagesAndRealObjectEstimationAreSourceGrounded()
    {
        var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit3EstimateMeasureCmTeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        var t=target.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("A, B and D",t.Explanation);
        Assert.Contains("C does NOT",t.Explanation);
        Assert.Contains("50–90 cm",t.Explanation);
        Assert.Contains("TRUE zero",t.QuickSummary);
        Assert.Contains("not the outer edge",string.Join(" ",t.KeyConceptsAndRules,t.StepByStepSolutions,t.CommonMistakes));
        Assert.Equal(CommonCoreG2Unit3EstimateMeasureCmTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(target.Doc,target.Lesson));
    }
    [Fact]
    public void WrongBodyShaOrPersistedContentVersionFailClosed()
    {
        var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit3EstimateMeasureCmTeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit3EstimateMeasureCmTeachingCorrection
            .CanUpgradeExisting(target.Doc,target.Lesson,"unreviewed"));
        var old=target.Lesson.CanonicalBodySha256;
        try {
            target.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit3EstimateMeasureCmTeachingCorrection.Apply(target.Doc));
        } finally {target.Lesson.CanonicalBodySha256=old;}
    }
}
