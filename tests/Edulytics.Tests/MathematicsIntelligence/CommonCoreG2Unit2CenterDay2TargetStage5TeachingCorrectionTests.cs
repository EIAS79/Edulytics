using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit2CenterDay2TargetStage5TeachingCorrectionTests
{
    [Fact]
    public void SourceThreeDiceAndEndRuleConflictAreNotFlattened()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit2CenterDay2TargetStage5TeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        var en=pair.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("66−28=38",string.Join(" ",en.Explanation,en.KeyConceptsAndRules,en.WorkedExamples,en.StepByStepSolutions,en.CommonMistakes,en.QuickSummary));
        Assert.Contains("73−24=49",string.Join(" ",en.Explanation,en.KeyConceptsAndRules,en.WorkedExamples,en.StepByStepSolutions,en.CommonMistakes,en.QuickSummary));
        Assert.Contains("73−52=21",string.Join(" ",en.Explanation,en.KeyConceptsAndRules,en.WorkedExamples,en.StepByStepSolutions,en.CommonMistakes,en.QuickSummary));
        Assert.Contains("THREE number cubes",en.Explanation);
        Assert.Contains("closest to zero after six rounds",en.Explanation);
        Assert.Contains("below10",en.Explanation);
        Assert.Equal(CommonCoreG2Unit2CenterDay2TargetStage5TeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(pair.Doc,pair.Lesson));
    }
    [Fact]
    public void OriginalHashAndOldVersionAreRequired()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit2CenterDay2TargetStage5TeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit2CenterDay2TargetStage5TeachingCorrection
            .CanUpgradeExisting(pair.Doc,pair.Lesson,"unreviewed"));
        var old=pair.Lesson.CanonicalBodySha256;
        try {
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit2CenterDay2TargetStage5TeachingCorrection.Apply(pair.Doc));
        } finally {pair.Lesson.CanonicalBodySha256=old;}
    }
}
