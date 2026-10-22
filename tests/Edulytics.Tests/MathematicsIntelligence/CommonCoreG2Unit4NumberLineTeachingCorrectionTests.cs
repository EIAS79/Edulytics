using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit4NumberLineTeachingCorrectionTests
{
    [Fact]
    public void OriginalSourceTenTickAndPhysicalTwentyTickContentIsBoundBySha()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit4NumberLineTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var english=row.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("3,4,6,7",english.Explanation);
        Assert.Contains("0–20",english.Explanation);
        Assert.Contains("13",english.StepByStepSolutions);
        Assert.Contains("make20 equal intervals",english.StepByStepSolutions);
        Assert.Contains("not the original assessment",english.Explanation);
        Assert.Equal(CommonCoreG2Unit4NumberLineTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(row.Doc,row.Lesson));
    }

    [Fact]
    public void UnknownPersistedPredecessorAndChangedOriginalSourceAreRejected()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit4NumberLineTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit4NumberLineTeachingCorrection
            .CanUpgradeExisting(row.Doc,row.Lesson,"unapproved"));
        var original=row.Lesson.CanonicalBodySha256;
        try
        {
            row.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit4NumberLineTeachingCorrection.Apply(row.Doc));
        }
        finally { row.Lesson.CanonicalBodySha256=original; }
    }
}
