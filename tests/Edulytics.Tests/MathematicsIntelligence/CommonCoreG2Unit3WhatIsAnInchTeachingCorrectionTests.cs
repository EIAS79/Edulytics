using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class CommonCoreG2Unit3WhatIsAnInchTeachingCorrectionTests
{
    [Fact]
    public void OriginalInchSourceRequiresRealPhysicalMeasurementAndNoInventedTriangleReading()
    {
        var p=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit3WhatIsAnInchTeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        var t=p.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("INCH",t.Explanation);
        Assert.Contains("inch tiles",t.Explanation);
        Assert.Contains("triangle",t.Explanation);
        Assert.Contains("between5 and6",t.WorkedExamples);
        Assert.Contains("do not record halves",t.Explanation);
        Assert.Equal(CommonCoreG2Unit3WhatIsAnInchTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(p.Doc,p.Lesson));
    }
    [Fact]
    public void SourceShaAndPersistedPredecessorCannotDrift()
    {
        var p=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Where(l=>
                CommonCoreG2Unit3WhatIsAnInchTeachingCorrection.IsTarget(doc,l))
                .Select(l=>(Doc:doc,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit3WhatIsAnInchTeachingCorrection.CanUpgradeExisting(
            p.Doc,p.Lesson,"unexpected"));
        var sha=p.Lesson.CanonicalBodySha256;
        try {
            p.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit3WhatIsAnInchTeachingCorrection.Apply(p.Doc));
        } finally {p.Lesson.CanonicalBodySha256=sha;}
    }
}
