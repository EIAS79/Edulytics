using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit2TapeStoriesCenterTeachingCorrectionTests
{
    [Fact]
    public void OriginalOptionalSourceRulesAndCompensationAreFaithful()
    {
        var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2TapeStoriesCenterTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var t=target.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("35−19=16",t.WorkedExamples);
        Assert.Contains("Tape Diagrams",t.Explanation);
        Assert.Contains("PRINTED",t.Explanation);
        Assert.Contains("do not invent",string.Join(" ",t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary),StringComparison.OrdinalIgnoreCase);
        Assert.Equal(CommonCoreG2Unit2TapeStoriesCenterTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(target.Doc,target.Lesson));
    }
    [Fact]
    public void ShaAndPriorVersionDriftAreFailClosed()
    {
        var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit2TapeStoriesCenterTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit2TapeStoriesCenterTeachingCorrection
            .CanUpgradeExisting(target.Doc,target.Lesson,"untrusted"));
        var before=target.Lesson.CanonicalBodySha256;
        try{
            target.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit2TapeStoriesCenterTeachingCorrection.Apply(target.Doc));
        }finally{target.Lesson.CanonicalBodySha256=before;}
    }
}
