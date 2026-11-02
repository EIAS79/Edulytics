using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit4UnlabeledTicksTeachingCorrectionTests
{
    [Fact]
    public void OriginalSourceNumbersAndImageSpecificEvidenceBoundaryAreGuarded()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit4UnlabeledTicksTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var en=row.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("24",en.Explanation);
        Assert.Contains("37",en.Explanation);
        Assert.Contains("48",en.Explanation);
        Assert.Contains("83",en.Explanation);
        Assert.Contains("17",en.Explanation);
        Assert.Contains("59",en.Explanation);
        Assert.Contains("43",en.Explanation);
        Assert.Contains("35",en.Explanation);
        Assert.Contains("cannot be accurately reconstructed",en.Explanation);
        Assert.Contains("zero",en.KeyConceptsAndRules,StringComparison.OrdinalIgnoreCase);
        Assert.Equal(CommonCoreG2Unit4UnlabeledTicksTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(row.Doc,row.Lesson));
    }
    [Fact]
    public void UnapprovedPersistedPredecessorAndChangedOriginalShaRejectSafely()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit4UnlabeledTicksTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit4UnlabeledTicksTeachingCorrection.CanUpgradeExisting(
            row.Doc,row.Lesson,"another-version"));
        var previous=row.Lesson.CanonicalBodySha256;
        try
        {
            row.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit4UnlabeledTicksTeachingCorrection.Apply(row.Doc));
        }
        finally{row.Lesson.CanonicalBodySha256=previous;}
    }
}
