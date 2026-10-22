using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal62OriginalSourceGuardTests
{
    [Fact]
    public void CanonicalSourceAndSixRepairedFieldsAreBoundToOriginalSha()
    {
        var x=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit4PlaceValueLineCorrection.IsTarget(d,l))
            .Select(l=>(Document:d,Lesson:l))).Single();
        var en=x.Lesson.Translations.Single(t=>t.CultureCode=="en");
        Assert.Contains("Source-accurate IM K",en.Explanation);
        Assert.Contains("First-party",en.StepByStepSolutions);
        Assert.Equal(CommonCoreG2Unit4PlaceValueLineCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(x.Document,x.Lesson));
    }
    [Fact]
    public void UnknownPersistedContentAndShaDriftFailClosed()
    {
        var x=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit4PlaceValueLineCorrection.IsTarget(d,l))
            .Select(l=>(Document:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit4PlaceValueLineCorrection.CanUpgradeExisting(x.Document,x.Lesson,"unapproved"));
        var original=x.Lesson.CanonicalBodySha256;
        try
        {
            x.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit4PlaceValueLineCorrection.Apply(x.Document));
        }
        finally { x.Lesson.CanonicalBodySha256=original; }
    }
}
