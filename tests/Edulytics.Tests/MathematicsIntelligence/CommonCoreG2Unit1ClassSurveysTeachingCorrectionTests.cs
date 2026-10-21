using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade 2 L18 source-guarded Rich V2 tests; authored, not run.</summary>
public sealed class CommonCoreG2Unit1ClassSurveysTeachingCorrectionTests
{
    [Fact]
    public void OriginalStudentSurveyTeachingIsMaterializedAndSourceSpecific()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit1ClassSurveysTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        var t=row.Lesson.Translations.Single(x=>x.CultureCode=="en");
        Assert.Contains("THREE or FOUR",t.StepByStepSolutions);
        Assert.Contains("FICTIONAL",t.WorkedExamples);
        Assert.Contains("tape diagram",t.Explanation);
        Assert.Contains("classmates",t.Explanation);
        Assert.Equal(CommonCoreG2Unit1ClassSurveysTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(row.Doc,row.Lesson));
    }
    [Fact]
    public void DriftedSourceIdentityAndUnauthorizedPriorVersionFailClosed()
    {
        var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG2Unit1ClassSurveysTeachingCorrection.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l))).Single();
        Assert.False(CommonCoreG2Unit1ClassSurveysTeachingCorrection
            .CanUpgradeExisting(row.Doc,row.Lesson,"unknown-version"));
        var previous=row.Lesson.CanonicalBodySha256;
        try {
            row.Lesson.CanonicalBodySha256=new string('f',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG2Unit1ClassSurveysTeachingCorrection.Apply(row.Doc));
        } finally {row.Lesson.CanonicalBodySha256=previous;}
    }
}
