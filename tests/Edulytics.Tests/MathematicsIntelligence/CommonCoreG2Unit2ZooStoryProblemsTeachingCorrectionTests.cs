using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Source-locked narrative and genuine zoo story types, D unrun.</summary>
public sealed class CommonCoreG2Unit2ZooStoryProblemsTeachingCorrectionTests
{
    [Fact]
    public void Original2021ZooFourNarrativesAndMathReplaceGenericText()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit2ZooStoryProblemsTeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        var en=pair.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("55−34=21",en.KeyConceptsAndRules);
        Assert.Contains("65−23=42",en.WorkedExamples);
        Assert.Contains("58−27=31",en.WorkedExamples);
        Assert.Contains("88−34=54",en.WorkedExamples);
        Assert.Contains("Three Reads",en.Explanation);
        Assert.Equal(CommonCoreG2Unit2ZooStoryProblemsTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(pair.Doc,pair.Lesson));
    }
    [Fact]
    public void UnrecognizedPredecessorAndAlteredCanonicalSourceCannotBeAccepted()
    {
        var pair=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit2ZooStoryProblemsTeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit2ZooStoryProblemsTeachingCorrection
            .CanUpgradeExisting(pair.Doc,pair.Lesson,"unrecognized"));
        var old=pair.Lesson.CanonicalBodySha256;
        try {
            pair.Lesson.CanonicalBodySha256=new string('f',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit2ZooStoryProblemsTeachingCorrection.Apply(pair.Doc));
        } finally {pair.Lesson.CanonicalBodySha256=old;}
    }
}
