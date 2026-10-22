using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit3StandardUnitsTeachingCorrectionTests
{
    [Fact]
    public void OriginalFortyCentimeterStringAndThreeMeasurementCountsAppear()
    {
        var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit3StandardUnitsTeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        var t=target.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("40",t.Explanation);
        Assert.Contains("10-cm straws",t.Explanation);
        Assert.Contains("5-cm straws",t.Explanation);
        Assert.Contains("centimeter cubes",t.Explanation);
        Assert.Contains("different",t.Explanation,StringComparison.OrdinalIgnoreCase);
        Assert.Equal(CommonCoreG2Unit3StandardUnitsTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(target.Doc,target.Lesson));
    }
    [Fact]
    public void ChangedOriginalHashAndUnreviewedPersistedVersionFailClosed()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit3StandardUnitsTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit3StandardUnitsTeachingCorrection
            .CanUpgradeExisting(pair.Doc,pair.Lesson,"not-approved"));
        var sha=pair.Lesson.CanonicalBodySha256;
        try {
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit3StandardUnitsTeachingCorrection.Apply(pair.Doc));
        } finally {pair.Lesson.CanonicalBodySha256=sha;}
    }
}
