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
        Assert.Contains("92−35=57",t.WorkedExamples);
        Assert.Contains("57cm−28cm=29cm",t.WorkedExamples);
        Assert.Contains("82−48=34",t.WorkedExamples);
        Assert.Contains("27+34=61",t.WorkedExamples);
        Assert.Contains("49+22=71",t.WorkedExamples);
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
