using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Grade2Ordinal56SourceGuardTests
{
    [Fact]
    public void OriginalSourceFieldsVersionAndImageOrClassBoundariesHold()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit4CompareNumberLinesCorrection.IsTarget(d,l))
            .Select(l=>(Document:d,Lesson:l))).Single();
        var english=row.Lesson.Translations.Single(t=>t.CultureCode=="en");
        Assert.Contains("THREE number cubes",english.Explanation);
        Assert.NotEmpty(english.StepByStepSolutions);
        Assert.Equal(CommonCoreG2Unit4CompareNumberLinesCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(row.Document,row.Lesson));
    }
    [Fact]
    public void ChangedOriginalBodyAndUnapprovedPersistedVersionsFailClosed()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit4CompareNumberLinesCorrection.IsTarget(d,l))
            .Select(l=>(Document:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit4CompareNumberLinesCorrection.CanUpgradeExisting(row.Document,row.Lesson,"arbitrary-unapproved"));
        var previous=row.Lesson.CanonicalBodySha256;
        try
        {
            row.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit4CompareNumberLinesCorrection.Apply(row.Document));
        }
        finally { row.Lesson.CanonicalBodySha256=previous; }
    }
}
