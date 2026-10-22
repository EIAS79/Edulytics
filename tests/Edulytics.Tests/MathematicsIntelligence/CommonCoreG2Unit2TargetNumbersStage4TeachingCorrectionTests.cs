using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Grade2 U02 L06 original teacher lesson via local read-only source; D unrun.</summary>
public sealed class CommonCoreG2Unit2TargetNumbersStage4TeachingCorrectionTests
{
    [Fact]
    public void SourceTrueFalse82AndTarget99AreMaterialized()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit2TargetNumbersStage4TeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        var en=pair.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("64=30+24",en.Explanation);
        Assert.Contains("FALSE",en.Explanation);
        Assert.Contains("Tyler",en.WorkedExamples);
        Assert.Contains("Target Numbers Stage4",en.Explanation);
        Assert.Contains("valid cards are integers1..9",en.KeyConceptsAndRules,StringComparison.OrdinalIgnoreCase);
        Assert.Equal(CommonCoreG2Unit2TargetNumbersStage4TeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(pair.Doc,pair.Lesson));
    }
    [Fact]
    public void SourceDriftOrUnknownPreviousVersionFailClosed()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit2TargetNumbersStage4TeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit2TargetNumbersStage4TeachingCorrection
            .CanUpgradeExisting(pair.Doc,pair.Lesson,"not-reviewed"));
        var sha=pair.Lesson.CanonicalBodySha256;
        try {
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit2TargetNumbersStage4TeachingCorrection.Apply(pair.Doc));
        } finally {pair.Lesson.CanonicalBodySha256=sha;}
    }
}
