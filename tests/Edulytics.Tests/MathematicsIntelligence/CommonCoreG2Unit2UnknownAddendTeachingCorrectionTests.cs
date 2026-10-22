using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Unrun Stage C authored checks for original G2 U02 L02 student learning.</summary>
public sealed class CommonCoreG2Unit2UnknownAddendTeachingCorrectionTests
{
    [Fact]
    public void OriginalChoralCountThreeMissingAddendsAndToolsAreMaterialized()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2UnknownAddendTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var t=pair.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("97,87,77",t.WorkedExamples);
        Assert.Contains("41+43=84",t.KeyConceptsAndRules);
        Assert.Contains("17+31=48",t.WorkedExamples);
        Assert.Contains("21+75=96",t.QuickSummary);
        Assert.Contains("base-ten blocks",t.Explanation);
        Assert.Equal(CommonCoreG2Unit2UnknownAddendTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(pair.Doc,pair.Lesson));
    }
    [Fact]
    public void OriginalSourceShaAndPersistedPredecessorMustMatch()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2UnknownAddendTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit2UnknownAddendTeachingCorrection
            .CanUpgradeExisting(pair.Doc,pair.Lesson,"unapproved"));
        var old=pair.Lesson.CanonicalBodySha256;
        try {
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit2UnknownAddendTeachingCorrection.Apply(pair.Doc));
        } finally {pair.Lesson.CanonicalBodySha256=old;}
    }
}
