using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class Grade2Ordinal59OriginalSourceGuardTests
{
    [Fact]
    public void OriginalLearnerSourceShaAndLessonVersionRemainBound()
    {
        var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit4DirectionalJumpCorrection.IsTarget(d,l))
            .Select(l=>(Doc:d,Lesson:l))).Single();
        var en=target.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("SOURCE",en.Explanation);
        Assert.NotEmpty(en.StepByStepSolutions);
        Assert.Equal(CommonCoreG2Unit4DirectionalJumpCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(target.Doc,target.Lesson));
    }
    [Fact]
    public void ChangedOriginalBodyAndUnknownPersistedPredecessorFailClosed()
    {
        var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit4DirectionalJumpCorrection.IsTarget(d,l))
            .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit4DirectionalJumpCorrection.CanUpgradeExisting(target.Doc,target.Lesson,"unapproved"));
        var sha=target.Lesson.CanonicalBodySha256;
        try
        {
            target.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit4DirectionalJumpCorrection.Apply(target.Doc));
        }
        finally{target.Lesson.CanonicalBodySha256=sha;}
    }
}
