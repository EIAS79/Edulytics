using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>G2 L17 original source guards; authored, NOT executed until D.</summary>
public sealed class CommonCoreG2Unit1CountingCollectionsCenterTeachingCorrectionTests
{
    [Fact]
    public void OptionalSourceCountAndTensTeachingActuallyMaterialized()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit1CountingCollectionsCenterTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var en=row.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("100–120",en.Explanation);
        Assert.Contains("Count",en.QuickSummary);
        Assert.Contains("Number Talk",en.WorkedExamples);
        Assert.Contains("Sort and Display",en.StepByStepSolutions);
        Assert.Equal(CommonCoreG2Unit1CountingCollectionsCenterTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(row.Doc,row.Lesson));
    }
    [Fact]
    public void OriginalSourceHashAndPriorVersionMustMatch()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit1CountingCollectionsCenterTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit1CountingCollectionsCenterTeachingCorrection
            .CanUpgradeExisting(row.Doc,row.Lesson,"not-an-approved-version"));
        var sha=row.Lesson.CanonicalBodySha256;
        try {
            row.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit1CountingCollectionsCenterTeachingCorrection.Apply(row.Doc));
        }
        finally {row.Lesson.CanonicalBodySha256=sha;}
    }
}
