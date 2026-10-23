using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit3PlantHeightsTeachingCorrectionTests
{
    [Fact]
    public void AuthenticGroupBAndHanImageEvidenceBoundarySurviveMaterialization()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit3PlantHeightsTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var en=pair.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("Andre33",en.Explanation);
        Assert.Contains("Clare25",en.Explanation);
        Assert.Contains("35−25=10",string.Join(" ",en.Explanation,en.KeyConceptsAndRules,en.WorkedExamples,en.StepByStepSolutions,en.CommonMistakes,en.QuickSummary));
        Assert.Contains("not in available archive text",en.Explanation);
        Assert.Contains("Han",en.Explanation);
        Assert.Equal(CommonCoreG2Unit3PlantHeightsTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(pair.Doc,pair.Lesson));
    }
    [Fact]
    public void PreviousContentHashAndPersistedVersionMustMatch()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit3PlantHeightsTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit3PlantHeightsTeachingCorrection
            .CanUpgradeExisting(pair.Doc,pair.Lesson,"unreviewed"));
        var sha=pair.Lesson.CanonicalBodySha256;
        try {
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit3PlantHeightsTeachingCorrection.Apply(pair.Doc));
        } finally {pair.Lesson.CanonicalBodySha256=sha;}
    }
}
