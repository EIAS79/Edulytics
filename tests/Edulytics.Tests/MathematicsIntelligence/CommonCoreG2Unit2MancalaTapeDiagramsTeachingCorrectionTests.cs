using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit2MancalaTapeDiagramsTeachingCorrectionTests
{
    [Fact]
    public void SourceDistinguishesFourDiagramOnlyStoriesAndOptionalNumberCompanion()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2MancalaTapeDiagramsTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var t=pair.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("DO NOT solve",t.Explanation);
        Assert.Contains("54+16=70",t.WorkedExamples);
        Assert.Contains("54−16=38",t.WorkedExamples);
        Assert.Contains("Cards L and N",t.Explanation);
        Assert.Equal(CommonCoreG2Unit2MancalaTapeDiagramsTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(pair.Doc,pair.Lesson));
    }
    [Fact]
    public void SourceHashAndPersistedPredecessorMustMatch()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2MancalaTapeDiagramsTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit2MancalaTapeDiagramsTeachingCorrection
            .CanUpgradeExisting(pair.Doc,pair.Lesson,"unapproved"));
        var sha=pair.Lesson.CanonicalBodySha256;
        try {
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit2MancalaTapeDiagramsTeachingCorrection.Apply(pair.Doc));
        } finally {pair.Lesson.CanonicalBodySha256=sha;}
    }
}
