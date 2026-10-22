using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit3TargetMeasurementsTeachingCorrectionTests
{
    [Fact]
    public void Optional2021SourceKeepsCorrectScorerEightRoundsAndDigitCardCenter()
    {
        var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit3TargetMeasurementsTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var en=target.Lesson.Translations.Single(t=>t.CultureCode=="en");
        Assert.Contains("8 rounds",en.Explanation);
        Assert.Contains("partner who SAID STOP",en.Explanation);
        Assert.Contains("10 inches",en.Explanation);
        Assert.Contains("30 centimeters",en.Explanation);
        Assert.Contains("Number Puzzles Stage4",en.Explanation);
        Assert.Equal(CommonCoreG2Unit3TargetMeasurementsTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(target.Doc,target.Lesson));
    }
    [Fact]
    public void ChangedSourceHashAndOldVersionAreRejected()
    {
        var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit3TargetMeasurementsTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit3TargetMeasurementsTeachingCorrection
            .CanUpgradeExisting(target.Doc,target.Lesson,"unreviewed"));
        var prev=target.Lesson.CanonicalBodySha256;
        try
        {
            target.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit3TargetMeasurementsTeachingCorrection.Apply(target.Doc));
        } finally {target.Lesson.CanonicalBodySha256=prev;}
    }
}
