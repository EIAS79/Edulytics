using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit3FriendshipBraceletsTeachingCorrectionTests
{
    [Fact]
    public void OriginalSourceHasFourConnectedTwoStepAnswersAndCorrectUnits()
    {
        var p=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit3FriendshipBraceletsTeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        var t=p.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("92−35=57",string.Join(" ",t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary));
        Assert.Contains("57cm−28cm=29cm",string.Join(" ",t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary));
        Assert.Contains("82−48=34",string.Join(" ",t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary));
        Assert.Contains("27+34=61",string.Join(" ",t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary));
        Assert.Contains("49+22=71",string.Join(" ",t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary));
        Assert.Contains("centimeters",t.KeyConceptsAndRules);
        Assert.Equal(CommonCoreG2Unit3FriendshipBraceletsTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(p.Doc,p.Lesson));
    }
    [Fact]
    public void SourceDriftAndUnknownPreviousPersistenceVersionFailClosed()
    {
        var p=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit3FriendshipBraceletsTeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit3FriendshipBraceletsTeachingCorrection
            .CanUpgradeExisting(p.Doc,p.Lesson,"unapproved"));
        var original=p.Lesson.CanonicalBodySha256;
        try {
            p.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit3FriendshipBraceletsTeachingCorrection.Apply(p.Doc));
        } finally {p.Lesson.CanonicalBodySha256=original;}
    }
}
