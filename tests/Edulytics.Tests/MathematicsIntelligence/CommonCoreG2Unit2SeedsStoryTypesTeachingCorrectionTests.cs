using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit2SeedsStoryTypesTeachingCorrectionTests
{
    [Fact]
    public void FourAuthenticSeedStructuresHaveCorrectNumbersAndRoles()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2SeedsStoryTypesTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var en=pair.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("42−16=26",en.WorkedExamples);
        Assert.Contains("31−15=16",en.WorkedExamples);
        Assert.Contains("37−28=9",en.WorkedExamples);
        Assert.Contains("50−23=27",en.WorkedExamples);
        Assert.Contains("Compare Smaller Unknown",en.Explanation);
        Assert.Equal(CommonCoreG2Unit2SeedsStoryTypesTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(pair.Doc,pair.Lesson));
    }
    [Fact]
    public void UnreviewedPersistedBodyAndAlteredCanonicalShaAreRefused()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2SeedsStoryTypesTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit2SeedsStoryTypesTeachingCorrection
            .CanUpgradeExisting(pair.Doc,pair.Lesson,"unreviewed"));
        var sha=pair.Lesson.CanonicalBodySha256;
        try {
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit2SeedsStoryTypesTeachingCorrection.Apply(pair.Doc));
        } finally {pair.Lesson.CanonicalBodySha256=sha;}
    }
}
