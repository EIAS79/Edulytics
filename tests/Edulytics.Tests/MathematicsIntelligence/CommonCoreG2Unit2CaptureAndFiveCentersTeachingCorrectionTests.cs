using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Source distinction between no-regroup talk and Stage6 composing-ten sums.</summary>
public sealed class CommonCoreG2Unit2CaptureAndFiveCentersTeachingCorrectionTests
{
    [Fact]
    public void BothActual2021OptionalCentersAndDifferentTensRulesAreAuthored()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2CaptureAndFiveCentersTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var t=pair.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("66−30=36",t.WorkedExamples);
        Assert.Contains("Capture Squares",t.Explanation);
        Assert.Contains("Five in a Row",t.Explanation);
        Assert.Contains("INCLUDING sums that compose a ten",t.Explanation);
        Assert.Contains("25+8=33",t.WorkedExamples);
        Assert.Equal(CommonCoreG2Unit2CaptureAndFiveCentersTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(pair.Doc,pair.Lesson));
    }
    [Fact]
    public void OriginalBodyAndPriorPersistedVersionAreFailClosed()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2CaptureAndFiveCentersTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit2CaptureAndFiveCentersTeachingCorrection
            .CanUpgradeExisting(pair.Doc,pair.Lesson,"unapproved"));
        var old=pair.Lesson.CanonicalBodySha256;
        try {
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit2CaptureAndFiveCentersTeachingCorrection.Apply(pair.Doc));
        } finally {pair.Lesson.CanonicalBodySha256=old;}
    }
}
