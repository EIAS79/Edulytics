using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G2 U02 L01 exact IM2021 no-regroup source/learner text, D not run.</summary>
public sealed class CommonCoreG2Unit2AddSubtractCompareTeachingCorrectionTests
{
    [Fact]
    public void WrongRegroupExamplesReplacedWithSourceExactGraphAndCubeTrains()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2AddSubtractCompareTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var t=row.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("WITHOUT composing or decomposing a ten",t.Explanation);
        Assert.Contains("32−21=11",t.WorkedExamples);
        Assert.Contains("46+22=68",t.WorkedExamples);
        Assert.Contains("22+24=46",t.KeyConceptsAndRules);
        Assert.Contains("27+15",t.CommonMistakes);
        Assert.Equal(CommonCoreG2Unit2AddSubtractCompareTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(row.Doc,row.Lesson));
    }
    [Fact]
    public void SourceHashAndPersistedPredecessorMustMatch()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2AddSubtractCompareTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit2AddSubtractCompareTeachingCorrection
            .CanUpgradeExisting(row.Doc,row.Lesson,"arbitrary-version"));
        var previous=row.Lesson.CanonicalBodySha256;
        try {
            row.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit2AddSubtractCompareTeachingCorrection.Apply(row.Doc));
        } finally {row.Lesson.CanonicalBodySha256=previous;}
    }
}
