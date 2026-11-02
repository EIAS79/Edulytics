using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade scope regression: one grade, exactly 96 source-guarded
/// learner-teaching candidates in addition to the frozen historical 50.
/// This is not the final original-student-task/Practice/academic acceptance.</summary>
public sealed class UsCcssG1FullGrade96SourceTeachingCoverageTests
{
    [Fact]
    public void EveryOneOf96NewGrade1LessonsHasExactlyOneSourceGuardedTeachingPath()
    {
        var docs=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var g1=docs.Single(d=>d.Lessons.Any(x=>
            x.LessonCode=="PED:US-CCSS-MATH:G1:U03:L14"));
        var ordered=g1.Lessons.Where(l=>l.LessonCode.StartsWith(
            "PED:US-CCSS-MATH:G1:",StringComparison.Ordinal)).ToArray();
        Assert.Equal(146,ordered.Length);
        Assert.Equal("PED:US-CCSS-MATH:G1:U03:L14",ordered[50].LessonCode);
        Assert.Equal("PED:US-CCSS-MATH:G1:U08:L10",ordered[145].LessonCode);

        int newlyWritten=0, previouslyCorrected=0;
        foreach(var lesson in ordered.Skip(50))
        {
            var n=new[]{
                CommonCoreG1FullGradeStartingContentCorrections.IsTarget(g1,lesson),
                CommonCoreG1FullGradeFinalUnit4Corrections.IsTarget(g1,lesson),
                CommonCoreG1FullGradeUnit5ContentCorrections.IsTarget(g1,lesson),
                CommonCoreG1FullGradeUnit6To8ContentCorrections.IsTarget(g1,lesson)
            }.Count(x=>x);
            var historicalSourceRepair=CommonCoreG1SourceGroundedInstructionalCorrections.IsTarget(g1,lesson);
            Assert.True(n<=1,"Duplicate newly written lesson source overlay: "+lesson.LessonCode);
            Assert.Equal(1,n+(historicalSourceRepair?1:0));
            Assert.True(CanonicalLessonContentMaterializer.IsReviewedCorrectionTarget(g1,lesson),
                "No source-guarded effective learner lesson: "+lesson.LessonCode);
            if(n==1)newlyWritten++;
            else previouslyCorrected++;
        }
        Assert.Equal(79,newlyWritten);
        Assert.Equal(17,previouslyCorrected);
    }
    [Fact]
    public void EffectiveLearnerVersionsFor96AreNeverOriginalGenericSourceVersion()
    {
        var doc=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(d=>d.Lessons.Any(l=>l.LessonCode=="PED:US-CCSS-MATH:G1:U08:L10"));
        CanonicalLessonContentMaterializer.Materialize(doc);
        var remaining=doc.Lessons.Where(x=>x.LessonCode.StartsWith(
            "PED:US-CCSS-MATH:G1:",StringComparison.Ordinal)).Skip(50).ToArray();
        Assert.Equal(96,remaining.Length);
        foreach(var lesson in remaining)
        {
            var version=CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,lesson);
            Assert.NotEqual(CommonCoreRichContentCorrections.CorrectionContentVersion,version);
            var tr=lesson.Translations.Single(x=>x.CultureCode=="en");
            Assert.False(string.IsNullOrWhiteSpace(tr.Explanation));
            Assert.False(string.IsNullOrWhiteSpace(tr.WorkedExamples));
            Assert.False(string.IsNullOrWhiteSpace(tr.StepByStepSolutions));
        }
    }
}
