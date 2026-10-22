using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Locally sourced Grade2 U02 L05 source predecessor and pedagogy; D unrun.</summary>
public sealed class CommonCoreG2Unit2DecomposeTenTeachingCorrectionTests
{
    [Fact]
    public void G2Lesson23PreservesSource82AndDiegoWithTensExchange()
    {
        var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2DecomposeTenTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var learner=target.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("82−9=73",learner.WorkedExamples);
        Assert.Contains("55−20=35",learner.WorkedExamples);
        Assert.Contains("35−8=27",learner.StepByStepSolutions);
        Assert.Contains("7 tens and 12 ones",learner.Explanation);
        Assert.Equal(CommonCoreG2Unit2DecomposeTenTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(target.Doc,target.Lesson));
    }
    [Fact]
    public void MismatchedOriginalSourceAndPriorBodyCannotBeSilentlyOverwritten()
    {
        var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2DecomposeTenTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit2DecomposeTenTeachingCorrection
            .CanUpgradeExisting(target.Doc,target.Lesson,"an-unreviewed-version"));
        var old=target.Lesson.CanonicalBodySha256;
        try
        {
            target.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit2DecomposeTenTeachingCorrection.Apply(target.Doc));
        }
        finally {target.Lesson.CanonicalBodySha256=old;}
    }
}
