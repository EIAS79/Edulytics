using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit2TwoStepSeedStoriesTeachingCorrectionTests
{
    [Fact]
    public void OriginalAndreAndGroupTwoStepNumbersPreserved()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2TwoStepSeedStoriesTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var t=pair.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("25+17=42",t.WorkedExamples);
        Assert.Contains("42+35=77",t.WorkedExamples);
        Assert.Contains("53−24=29",t.WorkedExamples);
        Assert.Contains("61−39=22",t.WorkedExamples);
        Assert.Contains("24+17=41",t.WorkedExamples);
        Assert.Contains("29+22+41=92",t.WorkedExamples);
        Assert.Equal(CommonCoreG2Unit2TwoStepSeedStoriesTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(pair.Doc,pair.Lesson));
    }
    [Fact]
    public void UnapprovedPersistenceAndChangedSourceFailClosed()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2TwoStepSeedStoriesTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit2TwoStepSeedStoriesTeachingCorrection
            .CanUpgradeExisting(pair.Doc,pair.Lesson,"wrong-source-version"));
        var previous=pair.Lesson.CanonicalBodySha256;
        try{
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit2TwoStepSeedStoriesTeachingCorrection.Apply(pair.Doc));
        } finally {pair.Lesson.CanonicalBodySha256=previous;}
    }
}
